using FluentAssertions;
using Moq;
using Teashop.Backend.Domain.Cart.Entities;
using Teashop.Backend.Domain.Order.Entities;
using Xunit;

namespace Teashop.Backend.Tests.UnitTests.Domain.Order.Entities
{
    public class OrderEntityTests
    {
        [Fact]
        public void GetShippingFee()
        {
            var order = new OrderEntity
            {
                ChosenShippingMethod = new ShippingMethod() { Fee = 19.6789 }
            };

            order.GetShippingFee().Should().Be(19.68);
        }

        [Fact]
        public void GetPaymentFee()
        {
            var order = new OrderEntity
            {
                ChosenPaymentMethod = new PaymentMethod() { Fee = 19.6789 }
            };

            order.GetPaymentFee().Should().Be(19.68);
        }

        [Fact]
        public void CalculateTotalPrice()
        {
            var order = new OrderEntity
            {
                ChosenShippingMethod = new ShippingMethod() { Fee = 10.00 },
                ChosenPaymentMethod = new PaymentMethod() { Fee = 5.00 },
                Cart = Mock.Of<CartEntity>(c => c.GetPrice() == 15.6789)
            };

            order.CalculateTotalPrice();

            order.TotalPrice.Should().Be(30.68);
        }
    }
}
