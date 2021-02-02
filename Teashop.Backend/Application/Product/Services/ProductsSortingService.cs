using System.Collections.Generic;
using System.Linq;
using Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Application.Product.Services
{
    public class ProductsSortingService : IProductsSortingService
    {
        public List<ProductEntity> SortProductsDefault(List<ProductEntity> products)
        {
            return SortProductsByPriceThenName(products);
        }

        public List<ProductEntity> SortProductsByPriceThenName(List<ProductEntity> products)
        {
            return products
                .OrderBy(CalculateComparedValueForSorting)
                .ThenBy(p => p.Name)
                .ToList();
        }

        private double CalculateComparedValueForSorting(ProductEntity product)
        {
            return product.PricedByWeight()
                ? (product.Price / product.QuantityPerPrice) * 100
                : product.Price;
        }
    }
}
