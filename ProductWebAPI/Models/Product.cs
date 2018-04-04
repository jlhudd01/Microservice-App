using ProductWebAPI.DomainEvents;

namespace ProductWebAPI.Models
{
    public class Product : Entity
    {
        public int Id {get;set;}
        public string Name {get; private set;}
        public decimal Price {get; private set;}

        public Product() { }

        public Product(string name, decimal price, int id = 0)
        {
            Id = id;
            Name = name;
            Price = price;
        }

        public void UpdateProduct(string name, decimal price)
        {
            Name = name;
            Price = price;
            var updateProductDomainEvent = new ProductUpdateDomainEvent(this);
            AddDomainEvent(updateProductDomainEvent);
        }

        public void RemoveProduct()
        {
            var deleteProductDomainEvent = new ProductDeleteDomainEvent(this);
            AddDomainEvent(deleteProductDomainEvent);
        }
    }
}