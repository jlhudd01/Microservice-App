using ProductWebAPI.Models;

namespace ProductWebAPI.IntegrationEvents.Events
{
    public class ProductUpdateIntegrationEvent : IntegrationEvent
    {
        public Product Product {get; private set;}

        public ProductUpdateIntegrationEvent(Product product)
        {
            Product = product;
        }
    }
}