using System;

namespace Teashop.Backend.UI.Api.Product.Models
{
    public class PresentationalProduct
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int QuantityPerPrice { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public PresentationalBrewingInfo BrewingInfo { get; set; }
        public string[] Categories { get; set; }
    }
}
