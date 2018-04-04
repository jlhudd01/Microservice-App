using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductWebAPI.Models;

namespace ProductWebAPI.Contexts
{
    public class IntegrationEventLogContext : DbContext
    {
        public IntegrationEventLogContext(DbContextOptions<IntegrationEventLogContext> options) : base(options) { }

        public DbSet<IntegrationEventLogEntry> IntegrationEventLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IntegrationEventLogEntry>(ConfigureIntegrationEventLogEntry);
        }

        void ConfigureIntegrationEventLogEntry(EntityTypeBuilder<IntegrationEventLogEntry> builder)
        {
            builder.ToTable("IntegrationEventLog");

            builder.HasKey(e => e.ID);
        }
    }
}