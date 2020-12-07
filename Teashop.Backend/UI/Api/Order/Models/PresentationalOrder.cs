using Teashop.Backend.UI.Api.Cart.Models;

namespace Teashop.Backend.UI.Api.Order.Models
{
    public class PresentationalOrder
    {
        public PresentationalContactInfo ContactInfo { get; set; }
        public PresentationalAddress ShippingAddress { get; set; }
        public PresentationalShippingMethod ChosenShippingMethod { get; set; }
        public PresentationalPaymentMethod ChosenPaymentMethod { get; set; }
        public PresentationalCart Cart { get; set; }
        public double TotalPrice { get; set; }
        public double ShippingPrice { get; set; }
    }
}
