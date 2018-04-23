using System.Linq;
using FluentValidation;
using OrderWebAPI.Models;

namespace OrderWebAPI.Commands.Validators
{
    public class RemoveOrderItemFromOrderCommandValidator : AbstractValidator<RemoveOrderItemFromOrderCommand>
    {
        public RemoveOrderItemFromOrderCommandValidator()
        {
            RuleFor(x => x.OrderItem).Must(BeValidOrderItem).WithMessage("Must have valid Order Item");
            RuleFor(x => x.Order).Must(BeValidOrder).WithMessage("Must be valid order");
        }

        private bool BeValidOrder(Order order)
        {
            if (order.Id > 0
            && order.OrderItems.Any())
            {
                return true;
            }

            return false;
        }

        private bool BeValidOrderItem(OrderItem orderItem)
        {
            if (orderItem != null
            && orderItem.Id > 0
            && !string.IsNullOrEmpty(orderItem.Name)
            && orderItem.Price > 0.00m)
            {
                return true;
            }

            return false;
        }
    }
}