using MediatR;
using System;
using Teashop.Backend.Domain.Order.Entities;

namespace Teashop.Backend.Application.Order.Commands.PlaceOrder
{
    public class PlaceOrderCommand : IRequest<int>
    {
        public ContactInfo ContactInfo { get; set; }
        public Address ShippingAddress { get; set; }
        public Address BillingAddress { get; set; }
        public string ChosenShippingMethodName { get; set; }
        public string ChosenPaymentMethodName { get; set; }
        public PaymentCard PaymentCard { get ; set; }
        public Guid CartId { get; set; }
    }
}
