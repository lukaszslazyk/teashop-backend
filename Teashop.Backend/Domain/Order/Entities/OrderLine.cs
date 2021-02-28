using System;
using Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Domain.Order.Entities
{
    public class OrderLine
    {
        public Guid OrderLineId { get; set; } = Guid.NewGuid();
        public int OrderLineNo { get; set; }
        public Guid ProductId { get; set; }
        public ProductEntity Product { get; set; }
        public int Quantity { get; set; }
        public double Price { get; private set; }
        public Guid OrderId { get; set; }
        public OrderEntity Order { get; set; }

        public virtual void CalculatePrice()
        {
            Price = Product.Price * (Quantity / Product.QuantityPerPrice);
        }
    }
}
