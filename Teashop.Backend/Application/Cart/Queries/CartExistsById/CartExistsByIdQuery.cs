using MediatR;
using System;

namespace Teashop.Backend.Application.Cart.Queries.CartExistsById
{
    public class CartExistsByIdQuery : IRequest<bool>
    {
        public Guid CartId;
    }
}
