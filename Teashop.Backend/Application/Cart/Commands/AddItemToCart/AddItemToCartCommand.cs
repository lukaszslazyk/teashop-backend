using MediatR;
using System;

namespace Teashop.Backend.Application.Cart.Commands.AddItemToCart
{
    public class AddItemToCartCommand : IRequest<Unit>
    {
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
