using System.Threading;
using System.Threading.Tasks;
using MediatR;
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
            var product = new Product(request.Name, request.Price, request.Id);

            _productRepository.Delete(product);

            return await _productRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}