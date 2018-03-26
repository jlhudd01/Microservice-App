using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductWebAPI.Models;
using ProductWebAPI.Repositories;

namespace ProductWebAPI.Contexts
{
    public class ProductContext : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;
        private ProductContext(DbContextOptions<ProductContext> options)
        : base(options)
        {

        }

        public ProductContext(DbContextOptions<ProductContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
            .HasKey(x => x.Id);

            modelBuilder.Entity<Product>()
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _mediator.DispatchDomainEvnetsAsync(this);

            var result = await base.SaveChangesAsync();

            return true;
        }

        public DbSet<Product> Products { get; set;}
    }
}