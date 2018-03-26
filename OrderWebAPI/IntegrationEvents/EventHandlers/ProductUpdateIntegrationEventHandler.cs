using System.Threading.Tasks;
using OrderWebAPI.Repositories;
using OrderWebAPI.IntegrationEvents.EventHandlers;
using OrderWebAPI.IntegrationEvents.Events;
using OrderWebAPI.Models;

namespace OrderWebAPI.IntegrationEvents.EventHandlers
{
    public class ProductUpdateIntegrationEventHandler : IIntegrationEventHandler<ProductUpdateIntegrationEvent>
    {
        private readonly IOrderRepository _orderRepository;
        
        public ProductUpdateIntegrationEventHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task Handle(ProductUpdateIntegrationEvent @event)
        {
            var orderItem = new OrderItem(@event.Product.Id, @event.Product.Name, @event.Product.Price);
            var ordersToUpdate = _orderRepository.GetOrdersToUpdate(orderItem);
            foreach (var order in ordersToUpdate)
            {
                order.UpdateOrderItem(orderItem);
            }

            await _orderRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}