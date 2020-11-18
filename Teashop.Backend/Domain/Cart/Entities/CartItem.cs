using System;
using Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Domain.Cart.Entities
{
    public class CartItem
    {
        public Guid ItemId { get; set; } = Guid.NewGuid();
        public Guid ProductId { get; set; }
        public ProductEntity Product { get; set; }
        public int Quantity { get; set; }
        public Guid CartId { get; set; }
        public CartEntity Cart { get; set; }
        public DateTime CreatedAt { get; set; }

        public double GetPrice()
        {
            return Product.Price * (Quantity / Product.QuantityPerPrice);
        }
    }
}
