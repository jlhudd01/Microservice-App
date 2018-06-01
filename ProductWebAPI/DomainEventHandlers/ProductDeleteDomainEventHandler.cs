using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ProductWebAPI.DomainEvents;
using ProductWebAPI.IntegrationEvents;
using ProductWebAPI.IntegrationEvents.Events;

namespace ProductWebAPI.DomainEventHandlers
{
    public class ProductDeleteDomainEventHandler : INotificationHandler<ProductDeleteDomainEvent>
    {
        private readonly IProductsIntegrationEventService _productsIntegrationService;
        public ProductDeleteDomainEventHandler(IProductsIntegrationEventService productsIntegrationEventService)
        {
            _productsIntegrationService = productsIntegrationEventService;
        }

        public async Task Handle(ProductDeleteDomainEvent productDeleteDomainEvent, CancellationToken cancellationToken)
        {
            var productUpdateIntegrationEvent = new ProductDeleteIntegrationEvent(productDeleteDomainEvent.Product);
            await _productsIntegrationService.PublishThroughEventBusAsync(productUpdateIntegrationEvent);
        }
    }
}