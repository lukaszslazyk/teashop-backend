using System;
using System.Collections.Generic;
using System.Linq;

namespace Teashop.Backend.Domain.Cart.Entities
{
    public class CartEntity
    {
        public Guid CartId { get; set; } = Guid.NewGuid();
        public IList<CartItem> Items { get; set; } = new List<CartItem>();

        public virtual double GetPrice()
        {
            return Items
                .Aggregate(0.0, (i, item) => i + item.GetPrice());
        }
    }
}
