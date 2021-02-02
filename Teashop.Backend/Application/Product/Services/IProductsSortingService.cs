using System.Collections.Generic;
using Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Application.Product.Services
{
    public interface IProductsSortingService
    {
        List<ProductEntity> SortProductsDefault(List<ProductEntity> products);
        List<ProductEntity> SortProductsByPriceThenName(List<ProductEntity> products);
    }
}
