using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Application.Product.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductEntity>> GetAll();
        Task<IEnumerable<ProductEntity>> GetProductsInCategory(string categoryName);
        Task<IEnumerable<ProductEntity>> GetProductsInCategoryWithPagination(string categoryName, int pageIndex, int pageSize);
        Task<int> CountProductsInCategory(string categoryName);
        Task<ProductEntity> GetById(Guid productId);
        Task<bool> ExistsById(Guid productId);
        Task<bool> CategoryExistsByName(string categoryName);
    }
}
