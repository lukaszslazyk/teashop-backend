namespace Teashop.Backend.Domain.Order.Entities
{
    public class PaymentMethod
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public double Fee { get; set; }
        public int PaymentMethodNo { get; set; }
    }
}
