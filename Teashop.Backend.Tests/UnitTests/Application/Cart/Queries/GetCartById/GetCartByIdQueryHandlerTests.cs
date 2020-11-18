using FluentAssertions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Teashop.Backend.Application.Cart.Queries.GetCartById;
using Teashop.Backend.Application.Cart.Repositories;
using Teashop.Backend.Application.Commons.Exceptions;
using Teashop.Backend.Domain.Cart.Entities;
using Xunit;

namespace Teashop.Backend.Tests.UnitTests.Application.Cart.Queries.GetCartById
{
    public class GetCartByIdQueryHandlerTests
    {
        private readonly GetCartByIdQueryHandler _getCartByIdQueryHandler;
        private readonly Mock<ICartRepository> _cartRepository = new Mock<ICartRepository>();

        public GetCartByIdQueryHandlerTests()
        {
            _getCartByIdQueryHandler = new GetCartByIdQueryHandler(_cartRepository.Object);
        }

        [Fact]
        public async Task WhenCartWithGivenIdDoesNotExistThenThrowNotFoundException()
        {
            var cartId = Guid.NewGuid();
            var inputQuery = CreateQuery(cartId);
            _cartRepository.Setup(r => r.GetById(cartId))
                .ReturnsAsync(() => null);

            Func<Task> act = async () =>
                await _getCartByIdQueryHandler.Handle(inputQuery, new CancellationToken(false));

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task WhenCartWithGivenIdExistThenReturnCartWithItemsSortedByCreatedAt()
        {
            var cartId = Guid.NewGuid();
            var inputQuery = CreateQuery(cartId);
            var cartReturnedFromRepository = new CartEntity();
            var referenceTime = DateTime.Now;
            cartReturnedFromRepository.Items.Add(CreateItem(referenceTime.AddDays(3)));
            cartReturnedFromRepository.Items.Add(CreateItem(referenceTime.AddDays(1)));
            cartReturnedFromRepository.Items.Add(CreateItem(referenceTime.AddDays(2)));
            _cartRepository.Setup(r => r.GetById(cartId))
                .ReturnsAsync(cartReturnedFromRepository);

            var cartReturned = await _getCartByIdQueryHandler
                .Handle(inputQuery, new CancellationToken(false));

            cartReturned.Items[0].CreatedAt.Should().Be(referenceTime.AddDays(1));
            cartReturned.Items[1].CreatedAt.Should().Be(referenceTime.AddDays(2));
            cartReturned.Items[2].CreatedAt.Should().Be(referenceTime.AddDays(3));
        }

        private GetCartByIdQuery CreateQuery(Guid cartId)
        {
            return new GetCartByIdQuery
            {
                CartId = cartId,
            };
        }

        private CartItem CreateItem(DateTime createdAt)
        {
            return new CartItem
            {
                ProductId = Guid.NewGuid(),
                Quantity = 100,
                CreatedAt = createdAt,
            };
        }
    }
}
