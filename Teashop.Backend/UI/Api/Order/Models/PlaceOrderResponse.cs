using System;

namespace Teashop.Backend.UI.Api.Order.Models
{
    public class PlaceOrderResponse
    {
        public Guid OrderId { get; set; }
        public int OrderNumber { get; set; }
    }
}
