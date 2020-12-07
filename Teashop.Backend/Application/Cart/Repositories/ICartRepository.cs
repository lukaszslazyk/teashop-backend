using System;
using System.Threading.Tasks;
using Teashop.Backend.Domain.Cart.Entities;

namespace Teashop.Backend.Application.Cart.Repositories
{
    public interface ICartRepository
    {
        Task<CartEntity> GetById(Guid cartId);
        Task<bool> ExistsById(Guid cartId);
        Task Create(CartEntity cart);
        Task Update(CartEntity cart);
        Task UpdateWithAddedItem(CartEntity cart, CartItem item);
        Task DeleteItem(CartItem item);
    }
}
