using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OrderWebAPI.Infrastructure;
using OrderWebAPI.Repositories;

namespace OrderWebAPI.Commands
{
    public class RemoveOrderItemFromOrderCommandHandler : IRequestHandler<RemoveOrderItemFromOrderCommand, bool>
    {
         private readonly IMediator _mediator;
        private readonly IOrderRepository _orderRepository;

        public RemoveOrderItemFromOrderCommandHandler(IMediator mediator, IOrderRepository orderRepository)
        {
            _mediator = mediator;
            _orderRepository = orderRepository;
        }

        public async Task<bool> Handle(RemoveOrderItemFromOrderCommand request, CancellationToken cancellationToken)
        {
            var orderItem = request.OrderItem;
            var order = request.Order;

            var orderToUpdate = _orderRepository.GetOrder(order.Id);
            orderToUpdate.RemoveOrderItem(orderItem);

            return await _orderRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }

    public class RemoveOrderItemFromOrderIdentifiedCommandHanlder : IdentifiedCommandHandler<RemoveOrderItemFromOrderCommand, bool>
    {
        public RemoveOrderItemFromOrderIdentifiedCommandHanlder(IMediator mediator, IRequestManager requestManager) : base(mediator, requestManager) { }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true;
        }
    }
}