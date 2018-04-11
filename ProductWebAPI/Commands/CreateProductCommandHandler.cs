using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ProductWebAPI.Infrastructure;
using ProductWebAPI.Models;
using ProductWebAPI.Repositories;

namespace ProductWebAPI.Commands
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly IProductRepository _productRepository;

        public CreateProductCommandHandler(IMediator mediator, IProductRepository productRepository)
        {
            _mediator = mediator;
            _productRepository = productRepository;
        }

        public async Task<bool> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product(request.Name, request.Price);

            _productRepository.Add(product);

            return await _productRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }

    public class CreateProductIdentifiedCommandHandler : IdentifiedCommandHandler<CreateProductCommand, bool>
    {
        public CreateProductIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager) : base(mediator, requestManager) { }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true;
        }
    }
}