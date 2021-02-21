using System;

namespace Teashop.Backend.Application.Order.Commands.PlaceOrder
{
    public class PlaceOrderCommandResult
    {
        public Guid OrderId { get; set; }
        public int OrderNumber { get; set; }
    }
}
