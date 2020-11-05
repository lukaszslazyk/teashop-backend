using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Teashop.Backend.Application.Cart.Commands;
using Teashop.Backend.Application.Cart.Repositories;
using Teashop.Backend.Domain.Cart.Entities;
using NotFoundException = Teashop.Backend.Application.Commons.Exceptions.NotFoundException;

namespace Teashop.Backend.Application.Cart.Queries.GetCartById
{
    public class GetCartByIdQueryHandler : IRequestHandler<GetCartByIdQuery, CartEntity>
    {
        private readonly ICartRepository _cartRepository;
        private CartEntity _cart;

        public GetCartByIdQueryHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<CartEntity> Handle(GetCartByIdQuery request, CancellationToken cancellationToken)
        {
            await LoadCartFromRepository(request.CartId);
            if (!CartFound())
                ThrowNotFoundException();

            return _cart;
        }

        private async Task LoadCartFromRepository(Guid cartId)
        {
            _cart = await _cartRepository.GetById(cartId);
        }

        private bool CartFound()
        {
            return _cart != null;
        }

        private void ThrowNotFoundException()
        {
            throw new NotFoundException("Cart with given id does not exist.");
        }
    }
}
