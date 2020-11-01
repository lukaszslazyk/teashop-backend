using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Teashop.Backend.Application.Cart.Repositories;
using Teashop.Backend.Domain.Cart.Entities;

namespace Teashop.Backend.Application.Cart.Commands.UpdateItemQuantity
{
    public class UpdateItemQuantityCommandHandler : IRequestHandler<UpdateItemQuantityCommand, Unit>
    {
        private readonly ICartRepository _cartRepository;
        private CartEntity _cart;
        private CartItem _itemToUpdate;

        public UpdateItemQuantityCommandHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<Unit> Handle(UpdateItemQuantityCommand request, CancellationToken cancellationToken)
        {
            await SetupOperation(request);
            await UpdateItemQuantity(request.Quantity);

            return Unit.Value;
        }

        private async Task SetupOperation(UpdateItemQuantityCommand request)
        {
            await LoadCartWith(request.CartId);
            LoadItemToUpdate(request.ProductId);
        }

        private async Task LoadCartWith(Guid cartId)
        {
            _cart = await _cartRepository.GetById(cartId);
        }

        private void LoadItemToUpdate(Guid productId)
        {
            _itemToUpdate = _cart.Items
                .FirstOrDefault(item => item.ProductId == productId);
        }

        private async Task UpdateItemQuantity(int quantity)
        {
            _itemToUpdate.Quantity = quantity;
            await _cartRepository.Update(_cart);
        }
    }
}
