﻿using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Teashop.Backend.Application.Commons.Exceptions;
using Teashop.Backend.Application.Product.Queries.GetProductsInCategory;
using Teashop.Backend.Application.Product.Repositories;
using Teashop.Backend.Application.Product.Services;
using Teashop.Backend.Domain.Product.Entities;
using Xunit;

namespace Teashop.Backend.Tests.UnitTests.Application.Product.Queries.GetProductsInCategory
{
    public class GetProductsInCategoryQueryHandlerTests
    {
        private readonly GetProductsInCategoryQueryHandler _getProductsInCategoryQueryHandler;
        private readonly Mock<IProductRepository> _productRepository = new Mock<IProductRepository>();
        private readonly Mock<IProductsSortingService> _productsSortingService = new Mock<IProductsSortingService>();

        public GetProductsInCategoryQueryHandlerTests()
        {
            _getProductsInCategoryQueryHandler = new GetProductsInCategoryQueryHandler(
                _productRepository.Object,
                _productsSortingService.Object);
        }

        [Fact]
        public async Task WhenCategoryDoesNotExistThenThrowNotFoundException()
        {
            var inputQuery = CreateQuery("foo");
            _productRepository.Setup(r => r.CategoryExistsByName("foo"))
                .ReturnsAsync(false);

            Func<Task> act = async () =>
                await _getProductsInCategoryQueryHandler.Handle(inputQuery, new CancellationToken(false));

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task WhenPaginationWasNotQueriedThenLoadAllProductsInCategory()
        {
            var inputQuery = CreateQuery("foo");
            inputQuery.PageIndexQueried = false;
            inputQuery.PageSizeQueried = false;
            var returnedFromRepository = CreateProductsList();
            _productRepository.Setup(r => r.CategoryExistsByName("foo"))
                .ReturnsAsync(true);
            _productRepository.Setup(r => r.GetProductsInCategory("foo"))
                .ReturnsAsync(returnedFromRepository);
            _productsSortingService.Setup(r => r.SortProductsDefault(returnedFromRepository))
                .Returns(returnedFromRepository);

            var result = await _getProductsInCategoryQueryHandler
                .Handle(inputQuery, new CancellationToken(false));

            _productRepository.Verify(r => r.GetProductsInCategory("foo"), Times.Once());
            result.Items.Should().BeEquivalentTo(returnedFromRepository);
        }

        [Fact]
        public async Task WhenPaginationWaQueriedThenLoadProductsInCategoryWithPagination()
        {
            var inputQuery = CreateQuery("foo");
            inputQuery.PageIndexQueried = true;
            inputQuery.PageIndex = 0;
            inputQuery.PageSizeQueried = true;
            inputQuery.PageSize = 10;
            var returnedFromRepository = CreateProductsList();
            _productRepository.Setup(r => r.CategoryExistsByName("foo"))
                .ReturnsAsync(true);
            _productRepository.Setup(r => r.CountProductsInCategory("foo"))
                .ReturnsAsync(20);
            _productRepository.Setup(r => r.GetProductsInCategoryWithPagination("foo", 0, 10))
                .ReturnsAsync(returnedFromRepository);
            _productsSortingService.Setup(r => r.SortProductsDefault(returnedFromRepository))
                .Returns(returnedFromRepository);

            var result = await _getProductsInCategoryQueryHandler
                .Handle(inputQuery, new CancellationToken(false));

            _productRepository.Verify(r => r.GetProductsInCategoryWithPagination("foo", 0, 10), Times.Once());
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
            _productRepository.Setup(r => r.CategoryExistsByName("foo"))
                .ReturnsAsync(true);
            _productRepository.Setup(r => r.GetProductsInCategory("foo"))
                .ReturnsAsync(returnedFromRepository);
            _productsSortingService.Setup(s => s.SortProductsDefault(returnedFromRepository))
                .Returns(returnedFromSortingService);

            var result = await _getProductsInCategoryQueryHandler
                .Handle(inputQuery, new CancellationToken(false));

            _productsSortingService.Verify(s => s.SortProductsDefault(returnedFromRepository), Times.Once());
            result.Items.Should().BeEquivalentTo(returnedFromSortingService);
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
