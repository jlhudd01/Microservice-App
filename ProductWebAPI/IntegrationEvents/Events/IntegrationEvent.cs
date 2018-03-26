using System;

namespace ProductWebAPI.IntegrationEvents.Events
{
    public class IntegrationEvent
    {
        public IntegrationEvent()
        {
            ID = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        public Guid ID { get; }
        public DateTime CreationDate { get; }
    }
}