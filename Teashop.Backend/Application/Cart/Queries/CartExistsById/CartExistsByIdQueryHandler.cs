using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Teashop.Backend.Application.Cart.Repositories;

namespace Teashop.Backend.Application.Cart.Queries.CartExistsById
{
    public class CartExistsByIdQueryHandler : IRequestHandler<CartExistsByIdQuery, bool>
    {
        private readonly ICartRepository _cartRepository;

        public CartExistsByIdQueryHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<bool> Handle(CartExistsByIdQuery request, CancellationToken cancellationToken)
        {
            return await _cartRepository.ExistsById(request.CartId);
        }
    }
}
