using MediatR;
using ProductWebAPI.Models;

namespace ProductWebAPI.DomainEvents
{
    public class ProductDeleteDomainEvent : INotification
    {
        public Product Product {get; private set;}

        public ProductDeleteDomainEvent(Product product)
        {
            Product = product;
        }
    }
}