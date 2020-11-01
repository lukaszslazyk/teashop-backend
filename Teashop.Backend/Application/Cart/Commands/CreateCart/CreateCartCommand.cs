using MediatR;
using System;

namespace Teashop.Backend.Application.Cart.Commands.CreateCart
{
    public class CreateCartCommand : IRequest<Guid>
    { }
}
