using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
                .ToListAsync();
        }

        public async Task<bool> ExistsById(Guid productId)
        {
            return await _context
                .Products
                .AnyAsync(p => p.ProductId == productId);
        }
    }
}
