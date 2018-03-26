using System.Threading.Tasks;

namespace ProductWebAPI.IntegrationEvents.EventHandlers
{
    public interface IDynamicIntegrationEventHandler
    {
         Task Handle(dynamic eventData);
    }
}