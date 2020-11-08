using FluentAssertions;
using Teashop.Backend.Domain.Cart.Entities;
using Teashop.Backend.Domain.Product.Entities;
using Xunit;

namespace Teashop.Backend.Tests.UnitTests.Domain.Cart.Entities
{
    public class CartItemTests
    {
        [Theory]
        [InlineData(19.99, 100, 19.99, 100)]
        [InlineData(39.98, 200, 19.99, 100)]
        [InlineData(9.99, 1, 9.99, 1)]
        [InlineData(99.90, 10, 9.99, 1)]
        public void GetPrice(double expectedItemPrice, int quantity, double productPrice, int quantityPerPrice)
        {
            var item = new CartItem()
            {
                Quantity = quantity,
                Product = new ProductEntity()
                {
                    Price = productPrice,
                    QuantityPerPrice = quantityPerPrice
                }
            };

            item.GetPrice().Should().Be(expectedItemPrice);
        }
    }
}
