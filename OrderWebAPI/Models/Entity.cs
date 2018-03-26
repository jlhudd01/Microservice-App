using System.Collections.Generic;
using MediatR;

namespace OrderWebAPI.Models
{
    public abstract class Entity
    {
        private List<INotification> _domainEvents;
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents;

        public void AddDomainEvent(INotification item)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(item);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}