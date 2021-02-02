using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Application.Product.Repositories
{
    public interface IProductRepository
    {
        Task<List<ProductEntity>> GetAll();
        Task<List<ProductEntity>> GetProductsInCategory(string categoryName);
        Task<List<ProductEntity>> GetProductsInCategoryWithPagination(string categoryName, int pageIndex, int pageSize);
        Task<int> CountProductsInCategory(string categoryName);
        Task<List<ProductEntity>> GetProductsWithSearchPhrase(string phrase);
        Task<List<ProductEntity>> GetProductsWithSearchPhraseWithPagination(string phrase, int pageIndex, int pageSize);
        Task<int> CountProductsWithSearchPhrase(string phrase);
        Task<ProductEntity> GetById(Guid productId);
        Task<bool> ExistsById(Guid productId);
        Task<bool> CategoryExistsByName(string categoryName);
    }
}
