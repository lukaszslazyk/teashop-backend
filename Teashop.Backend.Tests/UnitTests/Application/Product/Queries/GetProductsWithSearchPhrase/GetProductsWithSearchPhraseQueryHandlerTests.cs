using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Teashop.Backend.Application.Product.Queries.GetProductsWithSearchPhrase;
using Teashop.Backend.Application.Product.Repositories;
using Teashop.Backend.Application.Product.Services;
using Teashop.Backend.Domain.Product.Entities;
using Xunit;

namespace Teashop.Backend.Tests.UnitTests.Application.Product.Queries.GetProductsWithSearchPhrase
{
    public class GetProductsWithSearchPhraseQueryHandlerTests
    {
        private readonly GetProductsWithSearchPhraseQueryHandler _getProductsWithSearchPhraseQueryHandler;
        private readonly Mock<IProductRepository> _productRepository = new Mock<IProductRepository>();
        private readonly Mock<IProductsSortingService> _productsSortingService = new Mock<IProductsSortingService>();

        public GetProductsWithSearchPhraseQueryHandlerTests()
        {
            _getProductsWithSearchPhraseQueryHandler = new GetProductsWithSearchPhraseQueryHandler(
                _productRepository.Object,
                _productsSortingService.Object);
        }

        [Fact]
        public async Task WhenPaginationWasNotQueriedThenLoadAllProductsWithSearchPhrase()
        {
            var inputQuery = CreateQuery("foo");
            inputQuery.PageIndexQueried = false;
            inputQuery.PageSizeQueried = false;
            var returnedFromRepository = CreateProductsList();
            _productRepository.Setup(r => r.GetProductsWithSearchPhrase("foo"))
                .ReturnsAsync(returnedFromRepository);
            _productsSortingService.Setup(r => r.SortProductsDefault(returnedFromRepository))
                .Returns(returnedFromRepository);

            var result = await _getProductsWithSearchPhraseQueryHandler
                .Handle(inputQuery, new CancellationToken(false));

            _productRepository.Verify(r => r.GetProductsWithSearchPhrase("foo"), Times.Once());
            result.Items.Should().BeEquivalentTo(returnedFromRepository);
        }

        [Fact]
        public async Task WhenPaginationWasQueriedThenLoadProductsWithSearchPhraseWithPagination()
        {
            var inputQuery = CreateQuery("foo");
            inputQuery.PageIndexQueried = true;
            inputQuery.PageIndex = 0;
            inputQuery.PageSizeQueried = true;
            inputQuery.PageSize = 10;
            var returnedFromRepository = CreateProductsList();
            _productRepository.Setup(r => r.CountProductsWithSearchPhrase("foo"))
                .ReturnsAsync(20);
            _productRepository.Setup(r => r.GetProductsWithSearchPhraseWithPagination("foo", 0, 10))
                .ReturnsAsync(returnedFromRepository);
            _productsSortingService.Setup(r => r.SortProductsDefault(returnedFromRepository))
                .Returns(returnedFromRepository);

            var result = await _getProductsWithSearchPhraseQueryHandler
                .Handle(inputQuery, new CancellationToken(false));

            _productRepository.Verify(r => r.GetProductsWithSearchPhraseWithPagination("foo", 0, 10), Times.Once());
            result.Items.Should().BeEquivalentTo(returnedFromRepository);
            result.PageIndex.Should().Be(0);
            result.PageSize.Should().Be(10);
            result.TotalCount.Should().Be(20);
        }

        [Fact]
        public async Task ReturnProductsSorted()
        {
            var inputQuery = CreateQuery("foo");
            inputQuery.PageIndexQueried = false;
            inputQuery.PageSizeQueried = false;
            var returnedFromRepository = CreateProductsList();
            var returnedFromSortingService = CreateProductsList();
            returnedFromSortingService.Reverse();
            _productRepository.Setup(r => r.GetProductsWithSearchPhrase("foo"))
                .ReturnsAsync(returnedFromRepository);
            _productsSortingService.Setup(r => r.SortProductsDefault(returnedFromRepository))
                .Returns(returnedFromSortingService);

            var result = await _getProductsWithSearchPhraseQueryHandler
                .Handle(inputQuery, new CancellationToken(false));

            _productsSortingService.Verify(s => s.SortProductsDefault(returnedFromRepository), Times.Once());
            result.Items.Should().BeEquivalentTo(returnedFromSortingService);
        }

        public GetProductsWithSearchPhraseQuery CreateQuery(string phrase)
        {
            return new GetProductsWithSearchPhraseQuery
            {
                Phrase = phrase
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

        public List<ProductEntity> CreateProductsList()
        {
            return new List<ProductEntity>
            {
                CreateProduct("foo", 9.99, 100),
                CreateProduct("123", 9.99, 100),
            };
        }
    }
}
