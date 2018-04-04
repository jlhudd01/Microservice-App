using OrderWebAPI.Models;

namespace OrderWebAPI.IntegrationEvents.Events
{
    public class ProductDeleteIntegrationEvent : IntegrationEvent
    {
        public Product Product {get; private set;}

        public ProductDeleteIntegrationEvent(Product product)
        {
            Product = product;
        }
    }
}