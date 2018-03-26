using System.Linq;
using System.Threading.Tasks;
using MediatR;
using ProductWebAPI.Models;

namespace ProductWebAPI.Contexts
{
    public static class MediatorExtension
    {
        public static async Task DispatchDomainEvnetsAsync(this IMediator mediator, ProductContext context)
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