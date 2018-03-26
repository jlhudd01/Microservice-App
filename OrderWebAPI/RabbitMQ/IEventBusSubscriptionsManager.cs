using System;
using System.Collections.Generic;
using OrderWebAPI.IntegrationEvents.EventHandlers;
using OrderWebAPI.IntegrationEvents.Events;

namespace OrderWebAPI.RabbitMQ
{
    public interface IEventBusSubscriptionsManager
    {
         bool IsEmpty { get; }
         event EventHandler<string> OnEventRemoved;
         void AddDynamicSubscription<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler;
         void AddSubscription<T, TH>()
            where TH : IIntegrationEventHandler<T>
            where T : IntegrationEvent;

        void RemoveDynamicSubscription<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler;
        void RemoveSubscription<T, TH>()
            where TH : IIntegrationEventHandler<T>
            where T : IntegrationEvent;

        bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent;
        bool HasSubscriptionsForEvent(string eventName);
        Type GetEventTypeByName(string eventName);
        void Clear();
        IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent;
        IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);
        string GetEventKey<T>();
    }
}