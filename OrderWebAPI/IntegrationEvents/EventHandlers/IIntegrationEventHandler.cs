using System.Threading.Tasks;
using OrderWebAPI.IntegrationEvents.Events;

namespace OrderWebAPI.IntegrationEvents.EventHandlers
{
    public interface IIntegrationEventHandler<in TIntgeratinEvent> : IIntegrationEventHandler
        where TIntgeratinEvent : IntegrationEvent
    {
         Task Handle(TIntgeratinEvent @event);
    }

    public interface IIntegrationEventHandler {}
}