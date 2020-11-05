using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Teashop.Backend.Application.Cart.Repositories;
using Teashop.Backend.Domain.Cart.Entities;
using NotFoundException = Teashop.Backend.Application.Commons.Exceptions.NotFoundException;

namespace Teashop.Backend.Application.Cart.Commands.RemoveItemFromCart
{
    public class RemoveItemFromCartCommandHandler : IRequestHandler<RemoveItemFromCartCommand, Unit>
    {
        private readonly ICartRepository _cartRepository;
        private CartEntity _cart;
        private CartItem _itemToRemove;

        public RemoveItemFromCartCommandHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<Unit> Handle(RemoveItemFromCartCommand request, CancellationToken cancellationToken)
        {
            await LoadCartWith(request.CartId);
            LoadItemToRemove(request.ProductId);
            if (ItemInCart())
                await RemoveItem();

            return Unit.Value;
        }

        private async Task LoadCartWith(Guid id)
        {
            _cart = await _cartRepository.GetById(id);
            if (_cart == null)
                throw new NotFoundException("Cart with given id does not exist.");
        }

        private void LoadItemToRemove(Guid productId)
        {
            _itemToRemove = _cart.Items
                .FirstOrDefault(item => item.ProductId == productId);
        }

        private bool ItemInCart()
        {
            return _itemToRemove != null;
        }

        private async Task RemoveItem()
        {
            await _cartRepository.DeleteItem(_itemToRemove);
        }
    }
}
