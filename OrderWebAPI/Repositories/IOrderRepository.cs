using System.Collections.Generic;
using OrderWebAPI.Models;

namespace OrderWebAPI.Repositories
{
    public interface IOrderRepository
    {
         IEnumerable<Order> Get();
         Order GetOrder(int orderId);
         IUnitOfWork UnitOfWork { get; }
         IEnumerable<Order> GetOrdersToUpdate(OrderItem orderItem);
    }
}