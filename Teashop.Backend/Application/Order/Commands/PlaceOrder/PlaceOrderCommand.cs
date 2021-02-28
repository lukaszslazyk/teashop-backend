using MediatR;
using System;
using System.Collections.Generic;
using Teashop.Backend.Domain.Order.Entities;

namespace Teashop.Backend.Application.Order.Commands.PlaceOrder
{
    public class PlaceOrderCommand : IRequest<PlaceOrderCommandResult>
    {
        public ContactInfo ContactInfo { get; set; }
        public Address ShippingAddress { get; set; }
        public Address BillingAddress { get; set; }
        public string ChosenShippingMethodName { get; set; }
        public string ChosenPaymentMethodName { get; set; }
        public PaymentCard PaymentCard { get ; set; }
        public List<PlaceOrderCommandOrderLine> OrderLines { get; set; }
    }

    public class PlaceOrderCommandOrderLine
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
