using System;

namespace Teashop.Backend.UI.Api.Cart.Models
{
    public class UpdateSessionCartItemQuantityRequest
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
