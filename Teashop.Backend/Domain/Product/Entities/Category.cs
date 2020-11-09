using System;

namespace Teashop.Backend.Domain.Product.Entities
{
    public class Category
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public Category ParentCategory { get; set; }
    }
}
