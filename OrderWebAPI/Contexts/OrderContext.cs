using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderWebAPI.Models;
using OrderWebAPI.Repositories;

namespace OrderWebAPI.Contexts
{
    public class OrderContext : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;

        public OrderContext(DbContextOptions<OrderContext> options)
        : base(options)
        {

        }

        public OrderContext(DbContextOptions<OrderContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemEntityTypeConfiguration());
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _mediator.DispatchDomainEvnetsAsync(this);

            var result = await base.SaveChangesAsync();

            return true;
        }

        public DbSet<Order> Orders { get; set;}
        public DbSet<OrderItem> OrderItems { get; set;}
    }
}