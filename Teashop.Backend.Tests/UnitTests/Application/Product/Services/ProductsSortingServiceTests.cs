using FluentAssertions;
using System.Collections.Generic;
using Teashop.Backend.Application.Product.Services;
using Teashop.Backend.Domain.Product.Entities;
using Xunit;

namespace Teashop.Backend.Tests.UnitTests.Application.Product.Services
{
    public class ProductsSortingServiceTests
    {
        private readonly IProductsSortingService _productsSortingService;

        public ProductsSortingServiceTests()
        {
            _productsSortingService = new ProductsSortingService();
        }

        [Fact]
        public void SortProductsByPriceThenName_WhenProductsPricedByWeightThenReturnProductsSortedByPrice()
        {
            var input = new List<ProductEntity>
            {
                CreateProduct("A", 100.0, 1),
                CreateProduct("B", 10.0, 1),
                CreateProduct("C", 50.0, 1),
            };

            var result = _productsSortingService.SortProductsByPriceThenName(input);

            var products = result.ToArray();
            products[0].Name.Should().Be("B");
            products[1].Name.Should().Be("C");
            products[2].Name.Should().Be("A");
        }

        [Fact]
        public void SortProductsByPriceThenName_WhenProductsNotPricedByWeightThenReturnProductsSortedByPricePerReferenceQuantityEqualTo100()
        {
            var input = new List<ProductEntity>
            {
                CreateProduct("A", 20.0, 50),
                CreateProduct("B", 20.0, 100),
                CreateProduct("C", 10.0, 100),
            };

            var result = _productsSortingService.SortProductsByPriceThenName(input);

            var products = result.ToArray();
            products[0].Name.Should().Be("C");
            products[1].Name.Should().Be("B");
            products[2].Name.Should().Be("A");
        }

        [Fact]
        public void SortProductsByPriceThenName_WhenProductsEitherPricedByWeightOrNotThenReturnProductsSortedAccordingly()
        {
            var input = new List<ProductEntity>
            {
                CreateProduct("A", 100.0, 1),
                CreateProduct("B", 10.0, 1),
                CreateProduct("C", 20.0, 50),
                CreateProduct("D", 20.0, 100),
            };

            var result = _productsSortingService.SortProductsByPriceThenName(input);

            var products = result.ToArray();
            products[0].Name.Should().Be("B");
            products[1].Name.Should().Be("D");
            products[2].Name.Should().Be("C");
            products[3].Name.Should().Be("A");
        }

        [Fact]
        public void SortProductsByPriceThenName_WhenProductsHaveSameComparedValueThenReturnProductsSortedByName()
        {
            var input = new List<ProductEntity>
            {
                CreateProduct("D", 100.0, 1),
                CreateProduct("C", 100.0, 1),
                CreateProduct("B", 5.0, 50),
                CreateProduct("A", 10.0, 100),
            };

            var result = _productsSortingService.SortProductsByPriceThenName(input);

            var products = result.ToArray();
            products[0].Name.Should().Be("A");
            products[1].Name.Should().Be("B");
            products[2].Name.Should().Be("C");
            products[3].Name.Should().Be("D");
        }

        private ProductEntity CreateProduct(string name, double price, int quantityPerPrice)
        {
            return new ProductEntity
            {
                Name = name,
                Price = price,
                QuantityPerPrice = quantityPerPrice
            };
        }
    }
}
