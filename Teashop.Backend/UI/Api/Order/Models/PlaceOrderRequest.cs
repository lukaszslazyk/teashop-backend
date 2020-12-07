﻿namespace Teashop.Backend.UI.Api.Order.Models
{
    public class PlaceOrderRequest
    {
        public RequestContactInfo ContactInfo { get; set; }
        public RequestAddress ShippingAddress { get; set; }
        public string ChosenShippingMethodName { get; set; }
        public string ChosenPaymentMethodName { get; set; }
        public RequestPaymentCard PaymentCard { get; set; }
    }
}
