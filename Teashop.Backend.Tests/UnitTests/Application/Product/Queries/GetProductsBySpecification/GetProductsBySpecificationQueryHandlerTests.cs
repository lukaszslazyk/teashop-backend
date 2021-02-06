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

        public GetProductsBySpecificationQueryHandlerTests()
        {
            _getProductsBySpecificationQueryHandler = new GetProductsBySpecificationQueryHandler(_productRepository.Object);
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
            var inputQuery = CreateQueryWithEmptySpecification();
            var returnedFromRepository = CreateProductList();
            _productRepository.Setup(r => r.GetProductsBySpecification(inputQuery.Specification))
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
            _productRepository.Setup(r => r.GetProductsBySpecification(inputQuery.Specification))
                .ReturnsAsync(returnedFromRepository);
            _productRepository.Setup(r => r.CountProductsBySpecification(inputQuery.Specification))
                .ReturnsAsync(20);

            var result = await _getProductsBySpecificationQueryHandler
                .Handle(inputQuery, new CancellationToken(false));

            result.TotalCount.Should().Be(20);
            result.PageIndex.Should().Be(0);
            result.PageSize.Should().Be(10);
            result.Items.Should().BeEquivalentTo(returnedFromRepository);
        }

        private GetProductsBySpecificationQuery CreateQueryWithEmptySpecification()
        {
            return new GetProductsBySpecificationQuery
            {
                Specification = new ProductsQuerySpecification()
            };
        }

        private GetProductsBySpecificationQuery CreateQueryWithCategoryName(string categoryName)
        {
            return new GetProductsBySpecificationQuery
            {
                Specification = new ProductsQuerySpecification
                {
                    CategoryNameQueried = true,
                    CategoryName = categoryName,
                }
            };
        }

        private GetProductsBySpecificationQuery CreateQueryWithPaginationQueried(int pageIndex, int pageSize)
        {
            return new GetProductsBySpecificationQuery
            {
                Specification = new ProductsQuerySpecification
                {
                    PageIndexQueried = true,
                    PageIndex = pageIndex,
                    PageSizeQueried = true,
                    PageSize = pageSize,
                }
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
