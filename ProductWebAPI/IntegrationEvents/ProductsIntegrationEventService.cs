using System;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ProductWebAPI.Contexts;
using ProductWebAPI.IntegrationEvents.Events;
using ProductWebAPI.LogService;
using ProductWebAPI.RabbitMQ;

namespace ProductWebAPI.IntegrationEvents
{
    public class ProductsIntegrationEventService : IProductsIntegrationEventService
    {
        private readonly IEventBus _eventBus;
        private readonly ProductContext _productContext;
        private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
        private readonly IIntegrationEventLogService _integrationEventLogService;
        public ProductsIntegrationEventService(IEventBus eventBus, ProductContext productContext, Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory)
        {
            _eventBus = eventBus;
            _productContext = productContext;
            _integrationEventLogServiceFactory = integrationEventLogServiceFactory;
            _integrationEventLogService = _integrationEventLogServiceFactory(_productContext.Database.GetDbConnection());
            
        }
        public async Task PublishThroughEventBusAsync(IntegrationEvent integrationEvent)
        {
            await SaveEventAndOrderingContextChangesAsync(integrationEvent);
            _eventBus.Publish(integrationEvent);
            await _integrationEventLogService.MarkEventAsPublishedAsync(integrationEvent);
        }

        private async Task SaveEventAndOrderingContextChangesAsync(IntegrationEvent evt)
        {
            await ResilientTransaction.New(_productContext)
                .ExecuteAsync(async () => {
                    await _productContext.SaveChangesAsync();
                    await _integrationEventLogService.SaveEventAsync(evt, _productContext.Database.CurrentTransaction.GetDbTransaction());
                });
        }
    }
}