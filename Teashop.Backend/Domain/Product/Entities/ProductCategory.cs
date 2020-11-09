using System;

namespace Teashop.Backend.Domain.Product.Entities
{
    public class ProductCategory
    {
        public Guid ProductId { get; set; }
        public ProductEntity Product { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
