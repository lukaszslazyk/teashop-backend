using Microsoft.AspNetCore.Http;
using Teashop.Backend.UI.Api.Cart.Session;

namespace Teashop.Backend.UI.Api.Commons.Session
{
    public class SessionHandler
    {
        private readonly SessionCartHandler _sessionCartHandler;

        public SessionHandler(SessionCartHandler sessionCartHandler)
        {
            _sessionCartHandler = sessionCartHandler;
        }

        public void CloseSessionCart(ISession session)
        {
            _sessionCartHandler.RemoveCartFromSession(session);
        }
    }
}
