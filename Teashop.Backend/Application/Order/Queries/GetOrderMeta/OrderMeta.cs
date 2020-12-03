using System.Collections.Generic;
using Teashop.Backend.Domain.Order.Entities;

namespace Teashop.Backend.Application.Order.Queries.GetOrderMeta
{
    public class OrderMeta
    {
        public IEnumerable<Country> Countries { get; set; }
        public IEnumerable<ShippingMethod> ShippingMethods { get; set; }
        public IEnumerable<PaymentMethod> PaymentMethods { get; set; }
    }
}
