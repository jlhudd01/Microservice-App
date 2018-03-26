using System.Threading.Tasks;
using OrderWebAPI.IntegrationEvents.Events;
using OrderWebAPI.RabbitMQ;

namespace OrderWebAPI.IntegrationEvents
{
    public class OrdersIntegrationEventService : IOrdersIntegrationEventService
    {
        private readonly IEventBus _eventBus;
        public OrdersIntegrationEventService(IEventBus eventBus)
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