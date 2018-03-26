using System.Collections.Generic;
using System.Linq;

namespace OrderWebAPI.Models
{
    public class Order : Entity
    {
        public int Id { get; set; }
        private readonly List<OrderItem> _orderItems;
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        public Order() { _orderItems = new List<OrderItem>(); }

        public Order(List<OrderItem> orderItems)
        {
            _orderItems = orderItems;
        }

        public void RemoveOrderItem(OrderItem orderItem)
        {
            var index = _orderItems.FindIndex(0, _orderItems.Count, x => x.ProductId == orderItem.ProductId);
            _orderItems.RemoveAt(index);
        }

        public void UpdateOrderItem(OrderItem orderItem)
        {
            _orderItems.Where(y => y.ProductId == orderItem.ProductId).ToList().ForEach(x => {
                x.UpdateOrderItem(orderItem.Name, orderItem.Price);
            });
        }
    }
}