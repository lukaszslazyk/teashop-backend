using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Teashop.Backend.Application.Cart.Commands.CreateCart;

namespace Teashop.Backend.UI.Api.Cart.Utils
{
    public class SessionCartHandler
    {
        private const string CartIdKey = "CartId";

        private readonly IMediator _mediator;

        public SessionCartHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public Guid GetSessionCartId(ISession session)
        {
            return Guid.Parse(session.GetString(CartIdKey));
        }

        public async Task EnsureSessionHasCart(ISession session)
        {
            if (!HasCart(session))
                AddCartTo(session, await CreateNewCart());
        }

        private bool HasCart(ISession session)
        {
            return !string.IsNullOrEmpty(session.GetString(CartIdKey));
        }

        private void AddCartTo(ISession session, Guid cartId)
        {
            session.SetString(CartIdKey, cartId.ToString());
        }

        private async Task<Guid> CreateNewCart()
        {
            return await _mediator.Send(new CreateCartCommand());
        }
    }
}
