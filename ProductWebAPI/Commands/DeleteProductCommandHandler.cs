using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ProductWebAPI.Infrastructure;
using ProductWebAPI.Models;
using ProductWebAPI.Repositories;

namespace ProductWebAPI.Commands
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly IProductRepository _productRepository;

        public DeleteProductCommandHandler(IMediator mediator, IProductRepository productRepository)
        {
            _mediator = mediator;
            _productRepository = productRepository;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = _productRepository.GetProduct(request.Id);
            product.RemoveProduct();

            _productRepository.Delete(product);

            return await _productRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }

    public class DeleteProductIdentifiedCommandHandler : IdentifiedCommandHandler<DeleteProductCommand, bool>
    {
        public DeleteProductIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager) : base(mediator, requestManager) { }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true;
        }
    }
}