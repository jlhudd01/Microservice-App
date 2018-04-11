using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProductWebAPI.Contexts;
using ProductWebAPI.IntegrationEvents.Events;
using ProductWebAPI.Models;

namespace ProductWebAPI.LogService
{
    public class IntegrationEventLogService : IIntegrationEventLogService
    {
        private readonly DbConnection _dbConnection;
        private readonly IntegrationEventLogContext _integrationEventLogContext;

        public IntegrationEventLogService(DbConnection dbConnection)
        {
            _dbConnection = dbConnection;
            _integrationEventLogContext = new IntegrationEventLogContext(
                new DbContextOptionsBuilder<IntegrationEventLogContext>()
                .UseSqlite(_dbConnection)
                .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning))
                .Options);
        }

        public Task MarkEventAsPublishedAsync(IntegrationEvent @event)
        {
            try
            {
                _integrationEventLogContext.Database.UseTransaction(null);
                var eventLogEntry = _integrationEventLogContext.IntegrationEventLogs.Single(e => e.ID == @event.ID.ToString());

                eventLogEntry.TimesSent++;
                eventLogEntry.State = EventStateEnum.Published;

                _integrationEventLogContext.IntegrationEventLogs.Update(eventLogEntry);

                return _integrationEventLogContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public Task SaveEventAsync(IntegrationEvent @event, DbTransaction transaction)
        {
            if (transaction == null)
            {
                Console.WriteLine("null transaction");
            }
            var eventLogEntry = new IntegrationEventLogEntry(@event);

            _integrationEventLogContext.Database.UseTransaction(transaction);

            _integrationEventLogContext.IntegrationEventLogs.Add(eventLogEntry);

            return _integrationEventLogContext.SaveChangesAsync();
        }
    }
}