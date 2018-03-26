using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderWebAPI.Models;

namespace OrderWebAPI.Contexts
{
    public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> orderConfiguration)
        {
             orderConfiguration
            .HasKey(x => x.Id);

            orderConfiguration
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

            orderConfiguration
            .Ignore(x => x.DomainEvents);

            orderConfiguration
            .Metadata
            .FindNavigation(nameof(Order.OrderItems))
            .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}