using System;
using System.Collections.Generic;
using System.Linq;

namespace Teashop.Backend.Domain.Cart.Entities
{
    public class CartEntity
    {
        public Guid CartId { get; set; } = Guid.NewGuid();
        public IList<CartItem> Items { get; private set; } = new List<CartItem>();

        public double GetPrice()
        {
            return Items
                .Aggregate(0, (i, item) => i + item.Quantity);
        }
    }
}
