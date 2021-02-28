using System;
using System.Collections.Generic;
using System.Linq;

namespace Teashop.Backend.Domain.Order.Entities
{
    public class OrderEntity
    {
        public Guid OrderId { get; set; } = Guid.NewGuid();
        public int OrderNumber { get; set; }
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
        public DateTime CreatedAt { get; set; }
        public List<OrderLine> OrderLines { get; private set; } = new List<OrderLine>();
        public double TotalPrice { get; private set; }
        public double SubtotalPrice { get; private set; }
        public double ShippingFee { get; private set; }
        public double PaymentFee { get; private set; }

        public void CalculatePrices() {
            CalculateSubtotalPrice();
            CalculateShippingFee();
            CalculatePaymentFee();
            CalculateTotalPrice();
        }

        private void CalculateSubtotalPrice()
        {
            OrderLines.ForEach(line => line.CalculatePrice());
            var subtotal = OrderLines
                .Aggregate(0.0, (i, line) => i + line.Price);
            SubtotalPrice = Math.Round(subtotal, 2);
        }

        private void CalculateShippingFee()
        {
            ShippingFee = Math.Round(ChosenShippingMethod.Fee, 2);
        }

        private void CalculatePaymentFee()
        {
            PaymentFee = Math.Round(ChosenPaymentMethod.Fee, 2);
        }

        private void CalculateTotalPrice()
        {
            TotalPrice = Math.Round(SubtotalPrice + ShippingFee + PaymentFee, 2);
        }
    }
}
