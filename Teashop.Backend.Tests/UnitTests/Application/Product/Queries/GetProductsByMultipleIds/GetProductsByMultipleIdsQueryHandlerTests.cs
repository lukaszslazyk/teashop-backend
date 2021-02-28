using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Teashop.Backend.Application.Commons.Exceptions;
using Teashop.Backend.Application.Product.Queries.GetProductsByMultipleIds;
using Teashop.Backend.Application.Product.Repositories;
using Teashop.Backend.Domain.Product.Entities;
using Xunit;

namespace Teashop.Backend.Tests.UnitTests.Application.Product.Queries.GetProductsByMultipleIds
{
    public class GetProductsByMultipleIdsQueryHandlerTests
    {
        private readonly GetProductsByMultipleIdsQueryHandler _getProductsByMultipleIdsQueryHandler;
        private readonly Mock<IProductRepository> _productRepository = new Mock<IProductRepository>();

        public GetProductsByMultipleIdsQueryHandlerTests()
        {
            _getProductsByMultipleIdsQueryHandler = new GetProductsByMultipleIdsQueryHandler(_productRepository.Object);
        }

        [Fact]
        public async Task WhenProductsWithGivenIdsExistThenReturnProducts()
        {
            var productId1 = Guid.NewGuid();
            var productId2 = Guid.NewGuid();
            var productIds = new List<Guid> { productId1, productId2 };
            var inputQuery = CreateQueryWith(productIds);
            var returnedFromRepository = new List<ProductEntity>
            {
                new ProductEntity
                {
                    ProductId = productId1,
                },
                new ProductEntity
                {
                    ProductId = productId2,
                },
            };
            _productRepository.Setup(r => r.GetByMultipleIds(productIds))
                .ReturnsAsync(returnedFromRepository);

            var returned = await _getProductsByMultipleIdsQueryHandler
                .Handle(inputQuery, default);

            returned.Should().BeEquivalentTo(returnedFromRepository);
        }

        [Fact]
        public async Task WhenSomeProductsWithGivenIdsDoNotExistThenThrowNotFoundException()
        {
            var productId1 = Guid.NewGuid();
            var productId2 = Guid.NewGuid();
            var productId3 = Guid.NewGuid();
            var productIds = new List<Guid> { productId1, productId2, productId3 };
            var inputQuery = CreateQueryWith(productIds);
            var returnedFromRepository = new List<ProductEntity>
            {
                new ProductEntity
                {
                    ProductId = productId3,
                },
            };
            _productRepository.Setup(r => r.GetByMultipleIds(productIds))
                .ReturnsAsync(returnedFromRepository);

            Func<Task> act = async () => await _getProductsByMultipleIdsQueryHandler
                .Handle(inputQuery, default);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Products with the following ids do not exist: {productId1}, {productId2}");
        }

        private GetProductsByMultipleIdsQuery CreateQueryWith(List<Guid> productIds)
        {
            return new GetProductsByMultipleIdsQuery
            {
                ProductIds = new List<Guid>(productIds)
            };
        }
    }
}
