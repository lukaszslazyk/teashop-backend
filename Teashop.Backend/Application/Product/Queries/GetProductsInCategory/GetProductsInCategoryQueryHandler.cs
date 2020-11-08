using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Teashop.Backend.Application.Product.Repositories;
using Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Application.Product.Queries.GetProductsInCategory
{
    public class GetProductsInCategoryQueryHandler : IRequestHandler<GetProductsInCategoryQuery, IEnumerable<ProductEntity>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductsInCategoryQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductEntity>> Handle(GetProductsInCategoryQuery request, CancellationToken cancellationToken)
        {
            return await _productRepository.GetProductsInCategory(request.CategoryName);
        }
    }
}
