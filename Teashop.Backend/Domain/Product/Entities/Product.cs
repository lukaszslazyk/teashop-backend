namespace Teashop.Backend.Domain.Product.Entities
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public float ReferencePrice { get; set; }
        public int ReferenceGrams { get; set; }
    }
}
