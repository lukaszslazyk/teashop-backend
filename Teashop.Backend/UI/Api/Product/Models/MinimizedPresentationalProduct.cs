using System;

namespace Teashop.Backend.UI.Api.Product.Models
{
    public class MinimizedPresentationalProduct
    {
        public Guid Id { get; set; }
        public int ProductNumber { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int QuantityPerPrice { get; set; }
        public string ImagePath { get; set; }
    }
}
