using FluentAssertions;
using Teashop.Backend.Domain.Cart.Entities;
using Xunit;

namespace Teashop.Backend.Tests.UnitTests.Domain.Cart.Entities
{
    public class CartTests
    {
        [Fact]
        public void WhenNoItemsInCartThenReturnZeroForGetPrice()
        {
            var cart = new CartEntity();

            cart.GetPrice().Should().Be(0);
        }

        [Fact]
        public void WhenItemsInCartThenReturnSumOfItemPricesForGetPrice()
        {
            var cart = new CartEntity();
            cart.Items.Add(new CartItem { Quantity = 5 });
            cart.Items.Add(new CartItem { Quantity = 10 });
            cart.Items.Add(new CartItem { Quantity = 15 });

            cart.GetPrice().Should().Be(30);
        }
    }
}
