using System;
using System.Collections.Generic;

namespace Teashop.Backend.UI.Api.Order.Models
{
    public class PresentationalOrder
    {
        public int OrderNumber { get; set; }
        public DateTime PlacementDate { get; set; }
        public PresentationalContactInfo ContactInfo { get; set; }
        public PresentationalAddress ShippingAddress { get; set; }
        public PresentationalAddress BillingAddress { get; set; } 
        public PresentationalShippingMethod ChosenShippingMethod { get; set; }
        public PresentationalPaymentMethod ChosenPaymentMethod { get; set; }
        public List<PresentationalOrderLine> OrderLines { get; set; }
        public double TotalPrice { get; set; }
        public double SubtotalPrice { get; set; }
        public double ShippingFee { get; set; }
        public double PaymentFee { get; set; }
    }
}
