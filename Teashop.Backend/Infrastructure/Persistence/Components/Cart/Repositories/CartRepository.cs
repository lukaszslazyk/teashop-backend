using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Teashop.Backend.Application.Cart.Repositories;
using Teashop.Backend.Domain.Cart.Entities;
using Teashop.Backend.Infrastructure.Persistence.Context;

namespace Teashop.Backend.Infrastructure.Persistence.Components.Cart.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CartEntity> GetById(Guid cartId)
        {
            return await _context
                .Carts
                .Where(c => c.CartId == cartId)
                .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsById(Guid cartId)
        {
            return await _context
                .Carts
                .AnyAsync(c => c.CartId == cartId);
        }

        public async Task Create(CartEntity cart)
        {
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        public async Task Update(CartEntity cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateWithAddedItem(CartEntity cart, CartItem item)
        {
            item.CreatedAt = DateTime.Now;
            _context.CartItems.Add(item);
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteItem(CartItem item)
        {
            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
