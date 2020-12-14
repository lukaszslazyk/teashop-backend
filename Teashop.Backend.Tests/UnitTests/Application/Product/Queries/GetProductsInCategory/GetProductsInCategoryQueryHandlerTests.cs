using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Teashop.Backend.Application.Product.Queries.GetProductsInCategory;
using Teashop.Backend.Application.Product.Repositories;
using Teashop.Backend.Domain.Product.Entities;
using Xunit;

namespace Teashop.Backend.Tests.UnitTests.Application.Product.Queries.GetProductsInCategory
{
    public class GetProductsInCategoryQueryHandlerTests
    {
        private readonly GetProductsInCategoryQueryHandler _getProductsInCategoryQueryHandler;
        private readonly Mock<IProductRepository> _productRepository = new Mock<IProductRepository>();

        public GetProductsInCategoryQueryHandlerTests()
        {
            _getProductsInCategoryQueryHandler = new GetProductsInCategoryQueryHandler(_productRepository.Object);
        }

        [Fact]
        public async Task WhenProductsPricedByWeightThenReturnProductsSortedByPrice()
        {
            var inputQuery = CreateQuery("foo");
            var productsReturnedFromRepository = new List<ProductEntity>
            {
                CreateProduct("A", 100.0, 1),
                CreateProduct("B", 10.0, 1),
                CreateProduct("C", 50.0, 1),
            };
            _productRepository.Setup(r => r.GetProductsInCategory("foo"))
                    .ReturnsAsync(productsReturnedFromRepository);

            var productsReturned = await _getProductsInCategoryQueryHandler
                .Handle(inputQuery, new CancellationToken(false));

            var products = productsReturned.ToArray();
            products[0].Name.Should().Be("B");
            products[1].Name.Should().Be("C");
            products[2].Name.Should().Be("A");
        }

        [Fact]
        public async Task WhenProductsNotPricedByWeightThenReturnProductsSortedByPricePerReferenceQuantityEqualTo100()
        {
            var inputQuery = CreateQuery("foo");
            var productsReturnedFromRepository = new List<ProductEntity>
            {
                CreateProduct("A", 20.0, 50),
                CreateProduct("B", 20.0, 100),
                CreateProduct("C", 10.0, 100),
            };
            _productRepository.Setup(r => r.GetProductsInCategory("foo"))
                    .ReturnsAsync(productsReturnedFromRepository);

            var productsReturned = await _getProductsInCategoryQueryHandler
                .Handle(inputQuery, new CancellationToken(false));

            var products = productsReturned.ToArray();
            products[0].Name.Should().Be("C");
            products[1].Name.Should().Be("B");
            products[2].Name.Should().Be("A");
        }

        [Fact]
        public async Task WhenProductsEitherPricedByWeightOrNotThenReturnProductsSortedAccordingly()
        {
            var inputQuery = CreateQuery("foo");
            var productsReturnedFromRepository = new List<ProductEntity>
            {
                CreateProduct("A", 100.0, 1),
                CreateProduct("B", 10.0, 1),
                CreateProduct("C", 20.0, 50),
                CreateProduct("D", 20.0, 100),
            };
            _productRepository.Setup(r => r.GetProductsInCategory("foo"))
                    .ReturnsAsync(productsReturnedFromRepository);

            var productsReturned = await _getProductsInCategoryQueryHandler
                .Handle(inputQuery, new CancellationToken(false));

            var products = productsReturned.ToArray();
            products[0].Name.Should().Be("B");
            products[1].Name.Should().Be("D");
            products[2].Name.Should().Be("C");
            products[3].Name.Should().Be("A");
        }

        [Fact]
        public async Task WhenProductsHaveSameComparedValueThenReturnProductsSortedByName()
        {
            var inputQuery = CreateQuery("foo");
            var productsReturnedFromRepository = new List<ProductEntity>
            {
                CreateProduct("D", 100.0, 1),
                CreateProduct("C", 100.0, 1),
                CreateProduct("B", 5.0, 50),
                CreateProduct("A", 10.0, 100),
            };
            _productRepository.Setup(r => r.GetProductsInCategory("foo"))
                    .ReturnsAsync(productsReturnedFromRepository);

            var productsReturned = await _getProductsInCategoryQueryHandler
                .Handle(inputQuery, new CancellationToken(false));

            var products = productsReturned.ToArray();
            products[0].Name.Should().Be("A");
            products[1].Name.Should().Be("B");
            products[2].Name.Should().Be("C");
            products[3].Name.Should().Be("D");
        }

        public GetProductsInCategoryQuery CreateQuery(string categoryName)
        {
            return new GetProductsInCategoryQuery
            {
                CategoryName = categoryName
            };
        }

        public ProductEntity CreateProduct(string name, double price, int quantityPerPrice)
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
