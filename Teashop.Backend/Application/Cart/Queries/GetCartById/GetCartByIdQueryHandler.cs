using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Teashop.Backend.Application.Cart.Commands;
using Teashop.Backend.Application.Cart.Repositories;
using Teashop.Backend.Domain.Cart.Entities;

namespace Teashop.Backend.Application.Cart.Queries.GetCartById
{
    public class GetCartByIdQueryHandler : IRequestHandler<GetCartByIdQuery, CartEntity>
    {
        private readonly ICartRepository _cartRepository;

        public GetCartByIdQueryHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<CartEntity> Handle(GetCartByIdQuery request, CancellationToken cancellationToken)
        {
            return await _cartRepository.GetById(request.CartId);
        }
    }
}
