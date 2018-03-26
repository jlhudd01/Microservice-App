using System.Threading.Tasks;
using ProductWebAPI.IntegrationEvents.Events;

namespace ProductWebAPI.IntegrationEvents
{
    public interface IProductsIntegrationEventService
    {
         void PublishThroughEventBus(IntegrationEvent integrationEvent);
    }
}