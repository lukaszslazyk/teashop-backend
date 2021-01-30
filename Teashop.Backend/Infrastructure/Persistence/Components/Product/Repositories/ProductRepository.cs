﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<ProductEntity>> GetAll()
        {
            return await _context
                .Products
                .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .Include(p => p.BrewingInfo)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductEntity>> GetProductsInCategory(string categoryName)
        {
            return await _context
                .Products
                .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .Include(p => p.BrewingInfo)
                .Where(p => p.ProductCategories
                    .Any(pc => pc.Category.Name == categoryName)
                )
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductEntity>> GetProductsInCategoryWithPagination(
            string categoryName,
            int pageIndex,
            int pageSize)
        {
            return await _context
                .Products
                .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .Include(p => p.BrewingInfo)
                .Where(p => p.ProductCategories
                    .Any(pc => pc.Category.Name == categoryName)
                )
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> CountProductsInCategory(string categoryName)
        {
            return await _context
                .Products
                .Where(p => p.ProductCategories
                    .Any(pc => pc.Category.Name == categoryName)
                )
                .CountAsync();
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
    }
}
