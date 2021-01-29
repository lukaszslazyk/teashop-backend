namespace Teashop.Backend.Domain.Order.Entities
{
    public class ShippingMethod
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public double Fee { get; set; }
        public int ShippingMethodNo { get; set; }
    }
}
