using FluentAssertions;
using Moq;
using Teashop.Backend.Domain.Cart.Entities;
using Xunit;

namespace Teashop.Backend.Tests.UnitTests.Domain.Cart.Entities
{
    public class CartEntityTests
    {
        [Fact]
        public void WhenNoItemsInCartThenReturnZeroForGetPrice()
        {
            var cart = new CartEntity();

            cart.GetPrice().Should().Be(0.0);
        }

        [Fact]
        public void WhenItemsInCartThenReturnSumOfItemPricesForGetPrice()
        {
            var cart = new CartEntity();
            cart.Items.Add(Mock.Of<CartItem>(ci => ci.GetPrice() == 10.0));
            cart.Items.Add(Mock.Of<CartItem>(ci => ci.GetPrice() == 20.0));

            cart.GetPrice().Should().Be(30.0);
        }
    }
}
