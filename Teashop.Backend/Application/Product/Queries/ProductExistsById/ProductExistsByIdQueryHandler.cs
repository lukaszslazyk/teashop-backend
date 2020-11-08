using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Teashop.Backend.Application.Product.Repositories;

namespace Teashop.Backend.Application.Product.Queries.ProductExistsById
{
    public class ProductExistsByIdQueryHandler : IRequestHandler<ProductExistsByIdQuery, bool>
    {
        private readonly IProductRepository _productRepository;

        public ProductExistsByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> Handle(ProductExistsByIdQuery request, CancellationToken cancellationToken)
        {
            return await _productRepository.ExistsById(request.ProductId);
        }
    }
}
