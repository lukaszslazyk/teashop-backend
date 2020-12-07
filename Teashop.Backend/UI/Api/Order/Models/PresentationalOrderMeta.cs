namespace Teashop.Backend.UI.Api.Order.Models
{
    public class PresentationalOrderMeta
    {
        public PresentationalCountry[] Countries { get; set; }
        public PresentationalShippingMethod[] ShippingMethods { get; set; }
        public PresentationalPaymentMethod[] PaymentMethods { get; set; }
    }
}
