using System.Threading.Tasks;
using OrderWebAPI.IntegrationEvents.Events;
using OrderWebAPI.Models;
using OrderWebAPI.Repositories;

namespace OrderWebAPI.IntegrationEvents.EventHandlers
{
    public class ProductDeleteIntegrationEventHandler : IIntegrationEventHandler<ProductDeleteIntegrationEvent>
    {
        private readonly IOrderRepository _orderRepository;
        
        public ProductDeleteIntegrationEventHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task Handle(ProductDeleteIntegrationEvent @event)
        {
            var orderItem = new OrderItem(@event.Product.Id, @event.Product.Name, @event.Product.Price);
            var ordersToUpdate = _orderRepository.GetOrdersToUpdate(orderItem);
            foreach (var order in ordersToUpdate)
            {
                order.RemoveOrderItem(orderItem);
            }

            await _orderRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}