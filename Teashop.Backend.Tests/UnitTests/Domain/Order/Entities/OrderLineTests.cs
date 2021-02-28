using FluentAssertions;
using Teashop.Backend.Domain.Order.Entities;
using Teashop.Backend.Domain.Product.Entities;
using Xunit;

namespace Teashop.Backend.Tests.UnitTests.Domain.Order.Entities
{
    public class OrderLineTests
    {
        [Theory]
        [InlineData(19.99, 100, 19.99, 100)]
        [InlineData(39.98, 200, 19.99, 100)]
        [InlineData(9.99, 1, 9.99, 1)]
        [InlineData(99.90, 10, 9.99, 1)]
        public void CalculatePrice(double expectedItemPrice, int quantity, double productPrice, int quantityPerPrice)
        {
            var orderLine = new OrderLine()
            {
                Quantity = quantity,
                Product = new ProductEntity()
                {
                    Price = productPrice,
                    QuantityPerPrice = quantityPerPrice
                }
            };

            orderLine.CalculatePrice();

            orderLine.Price.Should().Be(expectedItemPrice);
        }
    }
}
