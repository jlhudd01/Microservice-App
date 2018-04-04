using System.Data.Common;
using System.Threading.Tasks;
using ProductWebAPI.IntegrationEvents.Events;

namespace ProductWebAPI.LogService
{
    public interface IIntegrationEventLogService
    {
         Task SaveEventAsync(IntegrationEvent @event, DbTransaction transaction);
         Task MarkEventAsPublishedAsync(IntegrationEvent @event);
    }
}