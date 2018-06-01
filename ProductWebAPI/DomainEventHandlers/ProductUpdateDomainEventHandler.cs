using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ProductWebAPI.DomainEvents;
using ProductWebAPI.IntegrationEvents;
using ProductWebAPI.IntegrationEvents.Events;

namespace ProductWebAPI.DomainEventHandlers
{
    public class ProductUpdateDomainEventHandler : INotificationHandler<ProductUpdateDomainEvent>
    {
        private readonly IProductsIntegrationEventService _productsIntegrationService;
        public ProductUpdateDomainEventHandler(IProductsIntegrationEventService productsIntegrationEventService)
        {
            _productsIntegrationService = productsIntegrationEventService;
        }

        public async Task Handle(ProductUpdateDomainEvent productUpdateDomainEvent, CancellationToken cancellationToken)
        {
            var productUpdateIntegrationEvent = new ProductUpdateIntegrationEvent(productUpdateDomainEvent.Product);
            await _productsIntegrationService.PublishThroughEventBusAsync(productUpdateIntegrationEvent);
        }
    }
}