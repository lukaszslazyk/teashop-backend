using MediatR;
using System;

namespace Teashop.Backend.Application.Cart.Commands.RemoveItemFromCart
{
    public class RemoveItemFromCartCommand : IRequest<Unit>
    {
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
    }
}
