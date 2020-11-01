using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Teashop.Backend.Application.Cart.Repositories;
using Teashop.Backend.Domain.Cart.Entities;

namespace Teashop.Backend.Application.Cart.Commands.CreateCart
{
    public class CreateCartCommandHandler : IRequestHandler<CreateCartCommand, Guid>
    {
        private readonly ICartRepository _cartRepository;

        public CreateCartCommandHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<Guid> Handle(CreateCartCommand request, CancellationToken cancellationToken)
        {
            var cart = new CartEntity();
            await _cartRepository.Create(cart);

            return cart.CartId;
        }
    }
}
