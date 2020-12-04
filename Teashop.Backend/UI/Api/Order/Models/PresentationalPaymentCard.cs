namespace Teashop.Backend.UI.Api.Order.Models
{
    public class PresentationalPaymentCard
    {
        public string Number { get; set; }
        public string Name { get; set; }
        public string ExpirationDate { get; set; }
        public string SecurityCode { get; set; }
    }
}
