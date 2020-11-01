using System;

namespace Teashop.Backend.UI.Api.Cart.Models
{
    public class RemoveItemFromSessionCartRequest
    {
        public Guid ProductId { get; set; }
    }
}