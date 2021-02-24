using System.Collections.Generic;
using System.Linq;
using Teashop.Backend.Domain.Cart.Entities;
using Teashop.Backend.UI.Api.Cart.Models;
using Teashop.Backend.UI.Api.Product.Mappings;

namespace Teashop.Backend.UI.Api.Cart.Mappings
{
    public class CartMapper
    {
        private readonly ProductMapper _productMapper;

        public CartMapper(ProductMapper productMapper)
        {
            _productMapper = productMapper;
        }

        public PresentationalCart MapToPresentational(CartEntity cart)
        {
            return new PresentationalCart()
            {
                Items = MapToPresentationals(cart.Items)
            };
        }

        public PresentationalCartItem MapToPresentational(CartItem item)
        {
            return new PresentationalCartItem
            {
                Product = _productMapper.MapToMinimized(item.Product),
                Quantity = item.Quantity
            };
        }

        public IList<PresentationalCartItem> MapToPresentationals(IList<CartItem> items)
        {
            return items
                .Select(item => MapToPresentational(item))
                .ToList();
        }
    }
}
