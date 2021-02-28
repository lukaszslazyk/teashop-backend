using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Teashop.Backend.Application.Product.Queries.GetProductsBySpecification;
using Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Application.Product.Repositories
{
    public interface IProductRepository
    {
        Task<List<ProductEntity>> GetProductsBySpecification(ProductsQuerySpecification specification);
        Task<int> CountProductsBySpecification(ProductsQuerySpecification specification);
        Task<ProductEntity> GetById(Guid productId);
        Task<List<ProductEntity>> GetByMultipleIds(List<Guid> productIds);
        Task<ProductEntity> GetByProductNumber(int productNumber);
        Task<bool> ExistsById(Guid productId);
        Task<bool> CategoryExistsByName(string categoryName);
    }
}
