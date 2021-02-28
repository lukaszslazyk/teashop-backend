using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Teashop.Backend.Application.Product.Queries.GetProductsBySpecification;
using Teashop.Backend.Application.Product.Repositories;
using Teashop.Backend.Domain.Product.Entities;
using Teashop.Backend.Infrastructure.Persistence.Context;

namespace Teashop.Backend.Infrastructure.Persistence.Components.Product.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<ProductEntity>> GetProductsBySpecification(ProductsQuerySpecification specification)
        {
            var query = CreateGetProductsBySpecificationQuery(specification);
            if (specification.PageIndexQueried && specification.PageSizeQueried)
                query = AddPaginationFilter(specification.PageIndex, specification.PageSize, query);
            
            return query.ToListAsync();
        }

        public Task<int> CountProductsBySpecification(ProductsQuerySpecification specification)
        {
            return CreateGetProductsBySpecificationQuery(specification)
                .CountAsync();
        }

        private IQueryable<ProductEntity> CreateGetProductsBySpecificationQuery(ProductsQuerySpecification specification)
        {
            var query = GetBaseOfGetProductsBySpecificationQuery();
            if (specification.CategoryNameQueried)
                query = AddCategoryNameFilter(specification.CategoryName, query);
            if (specification.SearchPhraseQueried)
                query = AddSearchPhraseFilter(specification.SearchPhrase, query);

            return AddOrderByFilter(specification.SortOption, query);
        }

        public async Task<ProductEntity> GetById(Guid productId)
        {
            return await _context
                .Products
                .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .Include(p => p.BrewingInfo)
                .Where(p => p.ProductId == productId)
                .SingleOrDefaultAsync();
        }

        public async Task<List<ProductEntity>> GetByMultipleIds(List<Guid> productIds)
        {
            return await _context
                .Products
                .Where(p => productIds.Contains(p.ProductId))
                .ToListAsync();
        }

        public async Task<ProductEntity> GetByProductNumber(int productNumber)
        {
            return await _context
                .Products
                .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .Include(p => p.BrewingInfo)
                .Where(p => p.ProductNumber == productNumber)
                .SingleOrDefaultAsync();
        }

        public async Task<bool> ExistsById(Guid productId)
        {
            return await _context
                .Products
                .AnyAsync(p => p.ProductId == productId);
        }

        public async Task<bool> CategoryExistsByName(string categoryName)
        {
            return await _context
                .Categories
                .AnyAsync(c => c.Name == categoryName);
        }

        private IQueryable<ProductEntity> GetBaseOfGetProductsBySpecificationQuery()
        {
            return _context.Products;
        }

        private IQueryable<ProductEntity> AddCategoryNameFilter(string categoryName, IQueryable<ProductEntity> query)
        {
            return query.Where(p => p.ProductCategories
                .Any(pc => pc.Category.Name == categoryName)
            );
        }

        private IQueryable<ProductEntity> AddSearchPhraseFilter(string searchPhrase, IQueryable<ProductEntity> query)
        {
            return query.Where(p => EF.Functions.Like(p.Name, "%" + searchPhrase + "%"));
        }

        private IQueryable<ProductEntity> AddPaginationFilter(int pageIndex, int pageSize, IQueryable<ProductEntity> query)
        {
            return query
                .Skip(pageIndex * pageSize)
                .Take(pageSize);
        }
        private IQueryable<ProductEntity> AddOrderByFilter(SortOption sortOption, IQueryable<ProductEntity> query)
        {
            return sortOption switch
            {
                SortOption.PriceAsc => query.OrderBy(x => x.Price),
                SortOption.PriceDesc => query.OrderByDescending(x => x.Price),
                SortOption.NameAsc => query.OrderBy(x => x.Name),
                SortOption.NameDesc => query.OrderByDescending(x => x.Name),
                SortOption.Default => AddDefaultOrderByFilter(query),
                _ => AddDefaultOrderByFilter(query),
            };
        }
        private IQueryable<ProductEntity> AddDefaultOrderByFilter(IQueryable<ProductEntity> query)
        {
            return query.OrderBy(x => x.Price);
        }
    }
}
