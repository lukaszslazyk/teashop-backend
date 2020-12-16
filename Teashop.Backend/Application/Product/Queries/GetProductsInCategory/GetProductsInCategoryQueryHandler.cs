using MediatR;
using System.Linq;
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
        private IEnumerable<ProductEntity> _products;

        public GetProductsInCategoryQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductEntity>> Handle(GetProductsInCategoryQuery request, CancellationToken cancellationToken)
        {
            await LoadProductsInCategory(request.CategoryName);
            SortProducts();

            return _products;
        }

        private async Task LoadProductsInCategory(string categoryName)
        {
            _products = await _productRepository.GetProductsInCategory(categoryName);
        }

        private void SortProducts()
        {
            _products = _products
                .OrderBy(CalculateComparedValueForSorting)
                .ThenBy(p => p.Name)
                .ToList();
        }

        private double CalculateComparedValueForSorting(ProductEntity product)
        {
            if (product.PricedByWeight())
                return (product.Price / product.QuantityPerPrice) * 100;
            else
                return product.Price;
        }
    }
}
