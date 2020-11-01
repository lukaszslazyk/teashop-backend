using MediatR;
using System;
using Teashop.Backend.Domain.Cart.Entities;

namespace Teashop.Backend.Application.Cart.Commands
{
    public class GetCartByIdQuery : IRequest<CartEntity>
    {
        public Guid CartId { get; set; }
    }
}
