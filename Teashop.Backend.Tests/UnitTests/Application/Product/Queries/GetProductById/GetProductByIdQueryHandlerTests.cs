using FluentAssertions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Teashop.Backend.Application.Commons.Exceptions;
using Teashop.Backend.Application.Product.Queries.GetProductById;
using Teashop.Backend.Application.Product.Repositories;
using Teashop.Backend.Domain.Product.Entities;
using Xunit;

namespace Teashop.Backend.Tests.UnitTests.Application.Product.Queries.GetProductById
{
    public class GetProductByIdQueryHandlerTests
    {
        private readonly GetProductByIdQueryHandler _getProductByIdQueryHandler;
        private readonly Mock<IProductRepository> _productRepository = new Mock<IProductRepository>();

        public GetProductByIdQueryHandlerTests()
        {
            _getProductByIdQueryHandler = new GetProductByIdQueryHandler(_productRepository.Object);
        }

        [Fact]
        public async Task WhenProductWithGivenIdDoesNotExistThenThrowNotFoundException()
        {
            var productId = Guid.NewGuid();
            var inputQuery = CreateQuery(productId);
            _productRepository.Setup(r => r.GetById(productId))
                .ReturnsAsync(() => null);

            Func<Task> act = async () =>
                await _getProductByIdQueryHandler.Handle(inputQuery, new CancellationToken(false));

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task WhenProductWithGivenIdExistThenReturnProduct()
        {
            var productId = Guid.NewGuid();
            var inputQuery = CreateQuery(productId);
            _productRepository.Setup(r => r.GetById(productId))
                .ReturnsAsync(CreateProduct(productId));

            var productReturned = await _getProductByIdQueryHandler
                .Handle(inputQuery, new CancellationToken(false));

            productReturned.ProductId.Should().Be(productId);
        }

        private GetProductByIdQuery CreateQuery(Guid productId)
        {
            return new GetProductByIdQuery
            {
                ProductId = productId
            };
        }

        private ProductEntity CreateProduct(Guid productId)
        {
            return new ProductEntity
            {
                ProductId = productId,
            };
        }
    }
}
