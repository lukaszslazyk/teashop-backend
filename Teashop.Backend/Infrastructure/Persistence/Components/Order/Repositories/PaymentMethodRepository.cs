using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Teashop.Backend.Application.Order.Repositories;
using Teashop.Backend.Domain.Order.Entities;
using Teashop.Backend.Infrastructure.Persistence.Context;

namespace Teashop.Backend.Infrastructure.Persistence.Components.Order.Repositories
{
    public class PaymentMethodRepository : IPaymentMethodRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentMethodRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PaymentMethod>> GetAll()
        {
            return await _context
                .PaymentMethods
                .OrderBy(pm => pm.PaymentMethodNo)
                .ToListAsync();
        }

        public async Task<PaymentMethod> GetByName(string name)
        {
            return await _context
                .PaymentMethods
                .Where(pm => pm.Name == name)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsByName(string name)
        {
            return await _context
                .PaymentMethods
                .AnyAsync(pm => pm.Name == name);
        }
    }
}
