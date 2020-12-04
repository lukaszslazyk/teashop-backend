namespace Teashop.Backend.UI.Api.Order.Models
{
    public class PlaceOrderRequest
    {
        public PresentationalContactInfo ContactInfo { get; set; }
        public PresentationalAddress ShippingAddress { get; set; }
        public string ChosenShippingMethodName { get; set; }
        public string ChosenPaymentMethodName { get; set; }
        public PresentationalPaymentCard PaymentCard { get; set; }
    }
}
