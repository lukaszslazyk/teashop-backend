using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Teashop.Backend.Application.Cart.Commands.RemoveItemFromCart;
using Teashop.Backend.Application.Cart.Repositories;
using Teashop.Backend.Application.Commons.Exceptions;
using Teashop.Backend.Domain.Cart.Entities;
using Xunit;

namespace Teashop.Backend.Tests.UnitTests.Application.Cart.Commands.RemoveItemFromCart
{
    public class RemoveItemFromCartHandlerTests
    {
        private readonly RemoveItemFromCartCommandHandler _removeItemFromCartCommandHandler;
        private readonly Mock<ICartRepository> _cartRepository = new Mock<ICartRepository>();

        public RemoveItemFromCartHandlerTests()
        {
            _removeItemFromCartCommandHandler = new RemoveItemFromCartCommandHandler(_cartRepository.Object);
        }

        [Fact]
        public async Task WhenCartWithGivenCartIdDoesNotExistThenThrowNotFoundException()
        {
            var cartId = Guid.NewGuid();
            var inputCommand = CreateCommand(cartId, Guid.NewGuid());
            _cartRepository.Setup(r => r.GetById(cartId))
                .ReturnsAsync(() => null);

            Func<Task> act = async () =>
                await _removeItemFromCartCommandHandler.Handle(inputCommand, default);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task WhenItemWithGivenProductIdNotInCartThenSkip()
        {
            var cartId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var inputCommand = CreateCommand(cartId, productId);
            var cartReturnedFromRepository = new CartEntity();
            cartReturnedFromRepository.Items.Add(CreateItem(Guid.NewGuid()));
            _cartRepository.Setup(r => r.GetById(cartId))
                .ReturnsAsync(cartReturnedFromRepository);

            await _removeItemFromCartCommandHandler.Handle(inputCommand, default);

            _cartRepository.Verify(x => x.DeleteItem(It.IsAny<CartItem>()), Times.Never());
        }

        [Fact]
        public async Task WhenItemWithGivenProductIdInCartThenRemoveItem()
        {
            var cartId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var inputCommand = CreateCommand(cartId, productId);
            var cartReturnedFromRepository = new CartEntity();
            cartReturnedFromRepository.Items.Add(CreateItem(Guid.NewGuid()));
            cartReturnedFromRepository.Items.Add(CreateItem(productId));
            CartItem itemOnDelete = null;
            _cartRepository.Setup(r => r.GetById(cartId))
                .ReturnsAsync(cartReturnedFromRepository);
            _cartRepository.Setup(r => r.DeleteItem(It.IsAny<CartItem>()))
                .Callback<CartItem>(c => itemOnDelete = c);

            await _removeItemFromCartCommandHandler.Handle(inputCommand, default);

            itemOnDelete.ProductId.Should().Be(productId);
        }

        private RemoveItemFromCartCommand CreateCommand(Guid cartId, Guid productId)
        {
            return new RemoveItemFromCartCommand
            {
                CartId = cartId,
                ProductId = productId
            };
        }

        private CartItem CreateItem(Guid productId)
        {
            return new CartItem
            {
                ProductId = productId
            };
        }
    }
}
