using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Teashop.Backend.Application.Cart.Repositories;
using Teashop.Backend.Domain.Cart.Entities;

namespace Teashop.Backend.Application.Cart.Commands.AddItemToCart
{
    public class AddItemToCartCommandHandler : IRequestHandler<AddItemToCartCommand, Unit>
    {
        private readonly ICartRepository _cartRepository;
        private CartEntity _cart;
        private CartItem _itemToAdd;
        private CartItem _itemFound;

        public AddItemToCartCommandHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<Unit> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
        {
            await SetupOperation(request);
            await ProcessOperation();

            return Unit.Value;
        }

        private async Task SetupOperation(AddItemToCartCommand request)
        {
            await LoadCartWith(request.CartId);
            CreateCartItemFrom(request);
        }

        private async Task LoadCartWith(Guid id)
        {
            _cart = await _cartRepository.GetById(id);
        }

        private void CreateCartItemFrom(AddItemToCartCommand request)
        {
            _itemToAdd = new CartItem
            {
                ProductId = request.ProductId,
                Quantity = request.Quantity
            };
        }

        private async Task ProcessOperation()
        {
            FindSameItemAlreadyInCart();
            if (SameItemFound())
                await AddQuantityToExistingItem();
            else
                await AddNewItemToCart();
        }

        private void FindSameItemAlreadyInCart()
        {
            _itemFound = _cart.Items
                .FirstOrDefault(item => item.ProductId == _itemToAdd.ProductId);
        }

        private bool SameItemFound()
        {
            return _itemFound != null;
        }

        private async Task AddQuantityToExistingItem()
        {
            _itemFound.Quantity += _itemFound.Quantity;
            await _cartRepository.Update(_cart);
        }

        private async Task AddNewItemToCart()
        {
            _cart.Items.Add(_itemToAdd);
            await _cartRepository.UpdateWithAddedItem(_cart, _itemToAdd);
        }
    }
}
