using System;
using System.Collections.Generic;

namespace Teashop.Backend.Domain.Product.Entities
{
    public class ProductEntity
    {
        public Guid ProductId { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public double Price { get; set; }
        public int QuantityPerPrice { get; set; }
        public IList<ProductCategory> Categories { get; private set; } = new List<ProductCategory>();
    }
}
