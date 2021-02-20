using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Teashop.Backend.Application.Commons.Exceptions;
using Teashop.Backend.Application.Product.Repositories;
using Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Application.Product.Queries.GetProductByProductNumber
{
    public class GetProductByProductNumberQueryHandler : IRequestHandler<GetProductByProductNumberQuery, ProductEntity>
    {
        private readonly IProductRepository _productRepository;
        private ProductEntity _product;

        public GetProductByProductNumberQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductEntity> Handle(GetProductByProductNumberQuery request, CancellationToken cancellationToken)
        {
            await LoadProductWith(request.ProductNumber);
            if (!ProductFound())
                ThrowNotFoundException();

            return _product;
        }

        private async Task LoadProductWith(int productNumber)
        {
            _product = await _productRepository.GetByProductNumber(productNumber);
        }

        private bool ProductFound()
        {
            return _product != null;
        }

        private void ThrowNotFoundException()
        {
            throw new NotFoundException("Product with given number does not exist.");
        }
    }
}
