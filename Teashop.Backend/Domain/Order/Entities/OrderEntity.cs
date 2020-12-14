using System;
using Teashop.Backend.Domain.Cart.Entities;

namespace Teashop.Backend.Domain.Order.Entities
{
    public class OrderEntity
    {
        public Guid OrderId { get; set; }
        public int OrderNo { get; set; }
        public Guid ContactInfoId { get; set; }
        public ContactInfo ContactInfo { get; set; }
        public Guid ShippingAddressId { get; set; }
        public Address ShippingAddress { get; set; }
        public Guid BillingAddressId { get; set; }
        public Address BillingAddress { get; set; }
        public string ChosenShippingMethodName { get; set; }
        public ShippingMethod ChosenShippingMethod { get; set; }
        public string ChosenPaymentMethodName { get; set; }
        public PaymentMethod ChosenPaymentMethod { get; set; }
        public Guid PaymentCardId { get; set; }
        public PaymentCard PaymentCard { get; set; }
        public Guid CartId { get; set; }
        public CartEntity Cart { get; set; }
        public DateTime CreatedAt { get; set; }

        public double GetTotalPrice()
        {
            return Cart.GetPrice() + GetShippingPrice();
        }

        public double GetShippingPrice()
        {
            return ChosenShippingMethod.Price;
        }
    }
}
