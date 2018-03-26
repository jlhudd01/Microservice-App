using System.Threading.Tasks;
using OrderWebAPI.IntegrationEvents.Events;

namespace OrderWebAPI.IntegrationEvents
{
    public interface IOrdersIntegrationEventService
    {
         void PublishThroughEventBus(IntegrationEvent integrationEvent);
    }
}