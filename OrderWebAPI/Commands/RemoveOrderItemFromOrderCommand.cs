using System.Runtime.Serialization;
using MediatR;
using OrderWebAPI.Models;

namespace OrderWebAPI.Commands
{
    [DataContract]
    public class RemoveOrderItemFromOrderCommand : IRequest<bool>
    {
        [DataMember]
        public Order Order {get;set;}
        [DataMember]
        public OrderItem OrderItem {get;set;}

        public RemoveOrderItemFromOrderCommand() { }

        public RemoveOrderItemFromOrderCommand(Order order, OrderItem orderItem)
        {
            Order = order;
            OrderItem = orderItem;
        }
    }
}