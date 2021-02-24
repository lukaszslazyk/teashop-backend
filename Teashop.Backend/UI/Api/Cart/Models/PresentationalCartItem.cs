using Teashop.Backend.UI.Api.Product.Models;

namespace Teashop.Backend.UI.Api.Cart.Models
{
    public class PresentationalCartItem
    {
        public MinimizedPresentationalProduct Product { get; set; }
        public int Quantity { get; set; }
    }
}
