using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OrderWebAPI.Contexts;
using OrderWebAPI.Models;

namespace OrderWebAPI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderContext _context;

        public IUnitOfWork UnitOfWork 
        {
            get
            {
                return _context;
            }
        }

        public OrderRepository(OrderContext context)
        {
            _context = context;
        }

        public IEnumerable<Order> Get()
        {
            return _context.Orders
            .Include(x => x.OrderItems)
            .ToList();
        }

        public Order GetOrder(int orderId)
        {
            return _context.Orders
                    .Where(x => x.Id == orderId)
                    .Include(x => x.OrderItems)
                    .FirstOrDefault();
        }

        public IEnumerable<Order> GetOrdersToUpdate(OrderItem orderItem)
        {
            return _context.Orders
                    .Where(x => x.OrderItems.Any(y => y.ProductId == orderItem.ProductId))
                    .Include(x => x.OrderItems)
                    .ToList();
        }
    }
}