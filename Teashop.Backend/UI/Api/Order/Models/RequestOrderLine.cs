using System;

namespace Teashop.Backend.UI.Api.Order.Models
{
    public class RequestOrderLine
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
