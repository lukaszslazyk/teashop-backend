using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Teashop.Backend.Application.Commons.Exceptions;
using Teashop.Backend.Application.Product.Queries.GetProductsBySpecification;
using Teashop.Backend.Application.Product.Repositories;
using Teashop.Backend.Domain.Product.Entities;
using Xunit;

namespace Teashop.Backend.Tests.UnitTests.Application.Product.Queries.GetProductsBySpecification
{
    public class GetProductsBySpecificationQueryHandlerTests
    {
        private readonly GetProductsBySpecificationQueryHandler _getProductsBySpecificationQueryHandler;
        private readonly Mock<IProductRepository> _productRepository = new Mock<IProductRepository>();
        private readonly Mock<ISortOptionNameParser> _sortOptionNameParser = new Mock<ISortOptionNameParser>();

        public GetProductsBySpecificationQueryHandlerTests()
        {
            _getProductsBySpecificationQueryHandler =
                new GetProductsBySpecificationQueryHandler(_productRepository.Object, _sortOptionNameParser.Object);
        }

        [Fact]
        public async Task WhenCategoryQueriedAndCategoryDoesNotExistThenThrowNotFoundException()
        {
            var inputQuery = CreateQueryWithCategoryName("foo");
            _productRepository.Setup(r => r.CategoryExistsByName("foo"))
                .ReturnsAsync(false);

            Func<Task> act = async () =>
                await _getProductsBySpecificationQueryHandler.Handle(inputQuery, new CancellationToken(false));

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task WhenPaginationWasNotQueriedThenReturnResultWithProductsReturnedFromRepository()
        {
            var inputQuery = new GetProductsBySpecificationQuery();
            var returnedFromRepository = CreateProductList();
            _productRepository.Setup(r => r.GetProductsBySpecification(It.IsAny<ProductsQuerySpecification>()))
                .ReturnsAsync(returnedFromRepository);

            var result = await _getProductsBySpecificationQueryHandler
                .Handle(inputQuery, new CancellationToken(false));

            result.Items.Should().BeEquivalentTo(returnedFromRepository);
        }

        [Fact]
        public async Task WhenPaginationWasQueriedThenReturnResultWithTotalNumberOfProductsThatMatchSpecification()
        {
            var inputQuery = CreateQueryWithPaginationQueried(0, 10);
            var returnedFromRepository = CreateProductList();
            _productRepository.Setup(r => r.GetProductsBySpecification(It.IsAny<ProductsQuerySpecification>()))
                .ReturnsAsync(returnedFromRepository);
            _productRepository.Setup(r => r.CountProductsBySpecification(It.IsAny<ProductsQuerySpecification>()))
                .ReturnsAsync(20);

            var result = await _getProductsBySpecificationQueryHandler
                .Handle(inputQuery, new CancellationToken(false));

            result.TotalCount.Should().Be(20);
            result.PageIndex.Should().Be(0);
            result.PageSize.Should().Be(10);
            result.Items.Should().BeEquivalentTo(returnedFromRepository);
        }

        [Fact]
        public async Task WhenOrderByWasQueriedThenMapToAccordingSortOptionInSpecification()
        {
            var inputQuery = CreateQueryWithOrderByQueried("exampleAsc");
            var returnedFromRepository = CreateProductList();
            ProductsQuerySpecification specificationOnRepositoryInput = null;
            _productRepository.Setup(r => r.GetProductsBySpecification(It.IsAny<ProductsQuerySpecification>()))
                .ReturnsAsync(returnedFromRepository)
                .Callback<ProductsQuerySpecification>(s => specificationOnRepositoryInput = s);
            _sortOptionNameParser.Setup(p => p.GetSortOptionFor("exampleAsc"))
                .Returns(SortOption.NameAsc);

            await _getProductsBySpecificationQueryHandler
                .Handle(inputQuery, new CancellationToken(false));

            specificationOnRepositoryInput.SortOption.Should().Be(SortOption.NameAsc);
        }

        [Fact]
        public async Task WhenOrderByWasNotQueriedThenMapToDefaultSortOptionInSpecification()
        {
            var inputQuery = new GetProductsBySpecificationQuery();
            var returnedFromRepository = CreateProductList();
            ProductsQuerySpecification specificationOnRepositoryInput = null;
            _productRepository.Setup(r => r.GetProductsBySpecification(It.IsAny<ProductsQuerySpecification>()))
                .ReturnsAsync(returnedFromRepository)
                .Callback<ProductsQuerySpecification>(s => specificationOnRepositoryInput = s);

            await _getProductsBySpecificationQueryHandler
                .Handle(inputQuery, new CancellationToken(false));

            specificationOnRepositoryInput.SortOption.Should().Be(SortOption.Default);
        }

        private GetProductsBySpecificationQuery CreateQueryWithCategoryName(string categoryName)
        {
            return new GetProductsBySpecificationQuery
            {
                CategoryNameQueried = true,
                CategoryName = categoryName,
            };
        }

        private GetProductsBySpecificationQuery CreateQueryWithPaginationQueried(int pageIndex, int pageSize)
        {
            return new GetProductsBySpecificationQuery
            {
                PageIndexQueried = true,
                PageIndex = pageIndex,
                PageSizeQueried = true,
                PageSize = pageSize,
            };
        }

        private GetProductsBySpecificationQuery CreateQueryWithOrderByQueried(string orderBy)
        {
            return new GetProductsBySpecificationQuery
            {
                OrderByQueried = true,
                OrderBy = orderBy,
            };
        }

        private List<ProductEntity> CreateProductList()
        {
            return new List<ProductEntity>
            {
                new ProductEntity
                {
                    Name = "abc"
                },
                new ProductEntity
                {
                    Name = "123"
                }
            };
        }
    }
}
