using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderWebAPI.Models;

namespace OrderWebAPI.Contexts
{
    public class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> orderItemConfiguration)
        {
            orderItemConfiguration.ToTable("OrderItems");

            orderItemConfiguration
            .HasKey(x => x.Id);

            orderItemConfiguration
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

            orderItemConfiguration
            .Ignore(x => x.DomainEvents);

            orderItemConfiguration.Property<int>("OrderId").IsRequired();
        }
    }
}