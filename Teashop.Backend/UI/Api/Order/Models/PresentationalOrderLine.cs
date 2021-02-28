using Teashop.Backend.UI.Api.Product.Models;

namespace Teashop.Backend.UI.Api.Order.Models
{
    public class PresentationalOrderLine
    {
        public MinimizedPresentationalProduct Product { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
