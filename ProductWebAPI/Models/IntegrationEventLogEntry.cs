using System;
using Newtonsoft.Json;
using ProductWebAPI.IntegrationEvents.Events;

namespace ProductWebAPI.Models
{
    public class IntegrationEventLogEntry
    {
        public IntegrationEventLogEntry() { }
        public IntegrationEventLogEntry(IntegrationEvent evt)
        {
            ID = evt.ID.ToString();
            CreationDate = evt.CreationDate;
            State = EventStateEnum.NotPublished;
            TimesSent = 0;
            Content = JsonConvert.SerializeObject(evt);
        }

        public string ID { get; private set; }
        public DateTime CreationDate { get; private set; }
        public EventStateEnum State {get; set; }
        public int TimesSent { get; set; }
        public string Content { get; private set; }
    }
}