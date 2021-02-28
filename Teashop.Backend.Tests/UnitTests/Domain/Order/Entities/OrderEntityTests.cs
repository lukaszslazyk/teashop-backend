using FluentAssertions;
using Moq;
using Teashop.Backend.Domain.Order.Entities;
using Xunit;

namespace Teashop.Backend.Tests.UnitTests.Domain.Order.Entities
{
    public class OrderEntityTests
    {
        [Fact]
        public void CalculateSubtotalPrice()
        {
            var order = new OrderEntity();
            order.OrderLines.Add(Mock.Of<OrderLine>(line => line.Price == 5.00));
            order.OrderLines.Add(Mock.Of<OrderLine>(line => line.Price == 4.99));
            order.OrderLines.Add(Mock.Of<OrderLine>(line => line.Price == 9.6789));
            order.ChosenShippingMethod = new ShippingMethod() { Fee = 0 };
            order.ChosenPaymentMethod = new PaymentMethod() { Fee = 0 };

            order.CalculatePrices();

            order.SubtotalPrice.Should().Be(19.67);
        }

        [Fact]
        public void CalculateShippingFee()
        {
            var order = new OrderEntity
            {
                ChosenShippingMethod = new ShippingMethod() { Fee = 19.6789 },
                ChosenPaymentMethod = new PaymentMethod() { Fee = 0 }
            };

            order.CalculatePrices();

            order.ShippingFee.Should().Be(19.68);
        }

        [Fact]
        public void CalculatePaymentFee()
        {
            var order = new OrderEntity
            {
                ChosenPaymentMethod = new PaymentMethod() { Fee = 19.6789 },
                ChosenShippingMethod = new ShippingMethod() { Fee = 0 },
            };

            order.CalculatePrices();

            order.PaymentFee.Should().Be(19.68);
        }

        [Fact]
        public void CalculateTotalPrice()
        {
            var order = new OrderEntity();
            order.OrderLines.Add(Mock.Of<OrderLine>(line => line.Price == 1.00));
            order.ChosenShippingMethod = new ShippingMethod() { Fee = 2.00 };
            order.ChosenPaymentMethod = new PaymentMethod() { Fee = 3.00 };

            order.CalculatePrices();

            order.TotalPrice.Should().Be(6.00);
        }
    }
}
