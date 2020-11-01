using System;

namespace Teashop.Backend.Domain.Product.Entities
{
    public class ProductEntity
    {
        public Guid ProductId { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public double Price { get; set; }
        public int QuantityPerPrice { get; set; }
    }
}
