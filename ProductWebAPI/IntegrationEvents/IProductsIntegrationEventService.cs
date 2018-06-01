using System.Threading.Tasks;
using ProductWebAPI.IntegrationEvents.Events;

namespace ProductWebAPI.IntegrationEvents
{
    public interface IProductsIntegrationEventService
    {
         Task PublishThroughEventBusAsync(IntegrationEvent integrationEvent);
    }
}