using System.Threading.Tasks;
using ProductWebAPI.IntegrationEvents.Events;
using ProductWebAPI.RabbitMQ;

namespace ProductWebAPI.IntegrationEvents
{
    public class ProductsIntegrationEventService : IProductsIntegrationEventService
    {
        private readonly IEventBus _eventBus;
        public ProductsIntegrationEventService(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }
        public void PublishThroughEventBus(IntegrationEvent integrationEvent)
        {
            //add event logging for tracking
            _eventBus.Publish(integrationEvent);
        }
    }
}