using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ProductWebAPI.Infrastructure;
using ProductWebAPI.Models;
using ProductWebAPI.Repositories;

namespace ProductWebAPI.Commands
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly IProductRepository _productRepository;

        public UpdateProductCommandHandler(IMediator mediator, IProductRepository productRepository)
        {
            _mediator = mediator;
            _productRepository = productRepository;
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = _productRepository.GetProduct(request.Id);
            product.UpdateProduct(request.Name, request.Price);

            _productRepository.Update(product);

            return await _productRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }

    public class UpdateProductIdentifiedCommandHandler : IdentifiedCommandHandler<UpdateProductCommand, bool>
    {
        public UpdateProductIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager) : base(mediator, requestManager) { }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true;
        }
    }
}