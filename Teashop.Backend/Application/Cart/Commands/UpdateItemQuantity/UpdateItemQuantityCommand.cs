using MediatR;
using System;

namespace Teashop.Backend.Application.Cart.Commands.UpdateItemQuantity
{
    public class UpdateItemQuantityCommand : IRequest<Unit>
    {
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
