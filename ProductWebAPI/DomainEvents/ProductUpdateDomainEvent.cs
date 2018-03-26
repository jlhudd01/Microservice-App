using MediatR;
using ProductWebAPI.Models;

namespace ProductWebAPI.DomainEvents
{
    public class ProductUpdateDomainEvent : INotification
    {
        public Product Product {get; private set;}

        public ProductUpdateDomainEvent(Product product)
        {
            Product = product;
        }
    }
}