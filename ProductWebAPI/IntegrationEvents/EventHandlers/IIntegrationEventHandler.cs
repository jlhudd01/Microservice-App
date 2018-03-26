using ProductWebAPI.IntegrationEvents.Events;

namespace ProductWebAPI.IntegrationEvents.EventHandlers
{
    public interface IIntegrationEventHandler<in TIntgeratinEvent> : IIntegrationEventHandler
        where TIntgeratinEvent : IntegrationEvent
    {
         
    }

    public interface IIntegrationEventHandler {}
}