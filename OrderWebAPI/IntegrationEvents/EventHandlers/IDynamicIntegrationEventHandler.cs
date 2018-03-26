using System.Threading.Tasks;

namespace OrderWebAPI.IntegrationEvents.EventHandlers
{
    public interface IDynamicIntegrationEventHandler
    {
         Task Handle(dynamic eventData);
    }
}