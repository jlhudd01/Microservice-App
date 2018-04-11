using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductWebAPI.Infrastructure;

namespace ProductWebAPI.Contexts
{
    public class ClientRequestEntityTypeConfiguration : IEntityTypeConfiguration<ClientRequest>
    {
        public void Configure(EntityTypeBuilder<ClientRequest> requestConfiguration)
        {
            requestConfiguration.ToTable("Requests");
            requestConfiguration.HasKey(x => x.Id);
            requestConfiguration.Property(x => x.Name).IsRequired();
            requestConfiguration.Property(x => x.Time).IsRequired();
        }
    }
}