using FluentAssertions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Teashop.Backend.Application.Cart.Commands.RemoveItemFromCart;
using Teashop.Backend.Application.Cart.Repositories;
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
        public async Task RemoveItemWithGivenProductId()
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

            await _removeItemFromCartCommandHandler.Handle(inputCommand, new CancellationToken(false));

            itemOnDelete.ProductId.Should().Be(productId);
        }

        public RemoveItemFromCartCommand CreateCommand(Guid cartId, Guid productId)
        {
            return new RemoveItemFromCartCommand
            {
                CartId = cartId,
                ProductId = productId
            };
        }

        public CartItem CreateItem(Guid productId)
        {
            return new CartItem
            {
                ProductId = productId
            };
        }
    }
}
