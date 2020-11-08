using FluentAssertions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Teashop.Backend.Application.Cart.Commands.UpdateItemQuantity;
using Teashop.Backend.Application.Cart.Repositories;
using Teashop.Backend.Application.Commons.Exceptions;
using Teashop.Backend.Domain.Cart.Entities;
using Xunit;

namespace Teashop.Backend.Tests.UnitTests.Application.Cart.Commands.UpdateItemQuantity
{
    public class UpdateItemQuantityCommandHandlerTests
    {
        private readonly UpdateItemQuantityCommandHandler _updateItemQuantityCommandHandler;
        private readonly Mock<ICartRepository> _cartRepository = new Mock<ICartRepository>();

        public UpdateItemQuantityCommandHandlerTests()
        {
            _updateItemQuantityCommandHandler = new UpdateItemQuantityCommandHandler(_cartRepository.Object);
        }

        [Fact]
        public async Task WhenCartWithGivenIdDoesNotExistThenThrowNotFoundException()
        {
            var cartId = Guid.NewGuid();
            var inputCommand = CreateCommand(cartId, Guid.NewGuid(), 100);
            _cartRepository.Setup(r => r.GetById(cartId))
                .ReturnsAsync(() => null);

            Func<Task> act = async () =>
                await _updateItemQuantityCommandHandler.Handle(inputCommand, new CancellationToken(false));

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task WhenItemWithGivenProductIdNotInCartThenThrowNotFoundException()
        {
            var cartId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var inputCommand = CreateCommand(cartId, productId, 100);
            var cartReturnedFromRepository = new CartEntity();
            cartReturnedFromRepository.Items.Add(CreateItem(Guid.NewGuid(), 125));
            _cartRepository.Setup(r => r.GetById(cartId))
                .ReturnsAsync(cartReturnedFromRepository);

            Func<Task> act = async () =>
                await _updateItemQuantityCommandHandler.Handle(inputCommand, new CancellationToken(false));

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task UpdateQuantityOfItemWithGivenProductId()
        {
            var cartId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var inputCommand = CreateCommand(cartId, productId, 50);
            var cartReturnedFromRepository = new CartEntity();
            cartReturnedFromRepository.Items.Add(CreateItem(Guid.NewGuid(), 125));
            cartReturnedFromRepository.Items.Add(CreateItem(productId, 100));
            CartEntity cartOnUpdate = null;
            _cartRepository.Setup(r => r.GetById(cartId))
                .ReturnsAsync(cartReturnedFromRepository);
            _cartRepository.Setup(r => r.Update(It.IsAny<CartEntity>()))
                .Callback<CartEntity>(c => cartOnUpdate = c);

            await _updateItemQuantityCommandHandler.Handle(inputCommand, new CancellationToken(false));

            cartOnUpdate.Items.Count.Should().Be(2);
            cartOnUpdate.Items[0].Quantity.Should().Be(125);
            cartOnUpdate.Items[1].Quantity.Should().Be(50);
        }

        public UpdateItemQuantityCommand CreateCommand(Guid cartId, Guid productId, int quantity)
        {
            return new UpdateItemQuantityCommand
            {
                CartId = cartId,
                ProductId = productId,
                Quantity = quantity
            };
        }

        public CartItem CreateItem(Guid productId, int quantity)
        {
            return new CartItem
            {
                ProductId = productId,
                Quantity = quantity
            };
        }
    }
}
