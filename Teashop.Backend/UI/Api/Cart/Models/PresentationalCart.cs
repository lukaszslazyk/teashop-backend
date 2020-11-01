using System.Collections.Generic;

namespace Teashop.Backend.UI.Api.Cart.Models
{
    public class PresentationalCart
    {
        public IList<PresentationalCartItem> Items { get; set; }
    }
}
