using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Teashop.Backend.Application.Order.Repositories;
using Teashop.Backend.Domain.Order.Entities;
using Teashop.Backend.Infrastructure.Persistence.Context;

namespace Teashop.Backend.Infrastructure.Persistence.Components.Order.Repositories
{
    public class ShippingMethodRepository : IShippingMethodRepository
    {
        private readonly ApplicationDbContext _context;

        public ShippingMethodRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ShippingMethod>> GetAll()
        {
            return await _context
                .ShippingMethods
                .OrderBy(sm => sm.ShippingMethodNo)
                .ToListAsync();
        }

        public async Task<ShippingMethod> GetByName(string name)
        {
            return await _context
                .ShippingMethods
                .Where(sm => sm.Name == name)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsByName(string name)
        {
            return await _context
                .ShippingMethods
                .AnyAsync(sm => sm.Name == name);
        }
    }
}
