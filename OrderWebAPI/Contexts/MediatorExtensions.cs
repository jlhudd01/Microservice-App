using System.Linq;
using System.Threading.Tasks;
using MediatR;
using OrderWebAPI.Models;

namespace OrderWebAPI.Contexts
{
    public static class MediatorExtensions
    {
        public static async Task DispatchDomainEvnetsAsync(this IMediator mediator, OrderContext context)
        {
            var domainEntities = context.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

            domainEntities.ToList()
            .ForEach(entity => entity.Entity.ClearDomainEvents());

            var tasks = domainEvents
            .Select(async (domainEvent) => {
                await mediator.Publish(domainEvent);
            });

            await Task.WhenAll(tasks);
        }
    }
}