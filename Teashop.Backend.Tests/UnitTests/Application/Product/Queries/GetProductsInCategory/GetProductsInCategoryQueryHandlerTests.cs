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

            var result = await _getProductsInCategoryQueryHandler
                .Handle(inputQuery, new CancellationToken(false));

            var products = result.Products.ToArray();
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

            var result = await _getProductsInCategoryQueryHandler
                .Handle(inputQuery, new CancellationToken(false));

            var products = result.Products.ToArray();
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

            var result = await _getProductsInCategoryQueryHandler
                .Handle(inputQuery, new CancellationToken(false));

            var products = result.Products.ToArray();
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

            var result = await _getProductsInCategoryQueryHandler
                .Handle(inputQuery, new CancellationToken(false));

            var products = result.Products.ToArray();
            products[0].Name.Should().Be("A");
            products[1].Name.Should().Be("B");
            products[2].Name.Should().Be("C");
            products[3].Name.Should().Be("D");
        }

        [Fact]
        public async Task WhenPaginationWasNotQueriedThenReturnResultWithPageSizeSetToNumberOfProdcutsInCategory()
        {
            var inputQuery = CreateQuery("foo");
            inputQuery.pageIndexQueried = false;
            inputQuery.pageSizeQueried = false;
            _productRepository.Setup(r => r.CountProductsInCategory("foo"))
                .ReturnsAsync(10);

            var result = await _getProductsInCategoryQueryHandler
                .Handle(inputQuery, new CancellationToken(false));

            result.PageIndex.Should().Be(0);
            result.PageSize.Should().Be(10);
            result.PagesInTotal.Should().Be(1);
        }

        [Fact]
        public async Task WhenPaginationWasQueriedThenReturnResultWithPagesInTotalCalculatedCorrectly()
        {
            var inputQuery = CreateQuery("foo");
            inputQuery.pageIndexQueried = true;
            inputQuery.pageIndex = 1;
            inputQuery.pageSizeQueried = true;
            inputQuery.pageSize = 2;
            _productRepository.Setup(r => r.CountProductsInCategory("foo"))
                .ReturnsAsync(10);

            var result = await _getProductsInCategoryQueryHandler
                .Handle(inputQuery, new CancellationToken(false));

            result.PageIndex.Should().Be(1);
            result.PageSize.Should().Be(2);
            result.PagesInTotal.Should().Be(5);
        }

        [Fact]
        public async Task WhenPaginationWasQueriedAndPageSizeIsLargerThanNumberOfProductsInCategoryThenReturnResultWithEqualToNumberOfProductsInCategory()
        {
            var inputQuery = CreateQuery("foo");
            inputQuery.pageIndexQueried = true;
            inputQuery.pageIndex = 0;
            inputQuery.pageSizeQueried = true;
            inputQuery.pageSize = 20;
            _productRepository.Setup(r => r.CountProductsInCategory("foo"))
                .ReturnsAsync(10);

            var result = await _getProductsInCategoryQueryHandler
                .Handle(inputQuery, new CancellationToken(false));

            result.PageIndex.Should().Be(0);
            result.PageSize.Should().Be(10);
            result.PagesInTotal.Should().Be(1);
        }

        [Fact]
        public async Task WhenPaginationWasQueriedAndPagesInTotalDivisionHasRemainderThenReturnResultWithPagesInTotalCalculatedCorrectly()
        {
            var inputQuery = CreateQuery("foo");
            inputQuery.pageIndexQueried = true;
            inputQuery.pageIndex = 2;
            inputQuery.pageSizeQueried = true;
            inputQuery.pageSize = 3;
            _productRepository.Setup(r => r.CountProductsInCategory("foo"))
                .ReturnsAsync(10);

            var result = await _getProductsInCategoryQueryHandler
                .Handle(inputQuery, new CancellationToken(false));

            result.PageIndex.Should().Be(2);
            result.PageSize.Should().Be(3);
            result.PagesInTotal.Should().Be(4);
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
