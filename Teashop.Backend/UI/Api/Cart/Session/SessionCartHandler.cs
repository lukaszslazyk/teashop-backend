using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Teashop.Backend.Application.Cart.Commands.CreateCart;

namespace Teashop.Backend.UI.Api.Cart.Session
{
    public class SessionCartHandler
    {
        private readonly string _cartIdKey = "CartId";

        private readonly IMediator _mediator;

        public SessionCartHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Guid GetSessionCartId(ISession session)
        {
            var sessionCartIdText = session.GetString(_cartIdKey);
            if (sessionCartIdText == null)
                throw new SessionCartIdNotSetException();

            return Guid.Parse(sessionCartIdText);
        }

        public async Task EnsureSessionHasCart(ISession session)
        {
            if (!HasCart(session))
                AddCartTo(session, await CreateNewCart());
        }

        public void RemoveCartFromSession(ISession session)
        {
            session.SetString(_cartIdKey, "");
        }

        private bool HasCart(ISession session)
        {
            return !string.IsNullOrEmpty(session.GetString(_cartIdKey));
        }

        private void AddCartTo(ISession session, Guid cartId)
        {
            session.SetString(_cartIdKey, cartId.ToString());
        }

        private async Task<Guid> CreateNewCart()
        {
            return await _mediator.Send(new CreateCartCommand());
        }
    }
}
