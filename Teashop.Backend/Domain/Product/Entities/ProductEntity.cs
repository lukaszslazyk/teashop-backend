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
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public BrewingInfo BrewingInfo { get; set; }
        public IList<ProductCategory> ProductCategories { get; private set; } = new List<ProductCategory>();

        public bool PricedByWeight()
        {
            return QuantityPerPrice != 1;
        }
    }
}
