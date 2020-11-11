using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Teashop.Backend.Application.Commons.Exceptions;
using Teashop.Backend.Application.Product.Repositories;
using Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Application.Product.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductEntity>
    {
        private readonly IProductRepository _productRepository;
        private ProductEntity _product;

        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductEntity> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            await LoadProductWith(request.ProductId);
            if (!ProductFound())
                ThrowNotFoundException();

            return _product;
        }

        private async Task LoadProductWith(Guid productId)
        {
            _product = await _productRepository.GetById(productId);
        }

        private bool ProductFound()
        {
            return _product != null;
        }

        private void ThrowNotFoundException()
        {
            throw new NotFoundException("Product with given id does not exist.");
        }
    }
}
