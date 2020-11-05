using FluentAssertions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Teashop.Backend.Application.Cart.Commands.AddItemToCart;
using Teashop.Backend.Application.Cart.Repositories;
using Teashop.Backend.Application.Commons.Exceptions;
using Teashop.Backend.Domain.Cart.Entities;
using Xunit;

namespace Teashop.Backend.Tests.UnitTests.Application.Cart.Commands.AddItemToCart
{
    public class AddItemToCartCommandHandlerTests
    {
        private readonly AddItemToCartCommandHandler _addItemToCartCommandHandler;
        private readonly Mock<ICartRepository> _cartRepository = new Mock<ICartRepository>();

        public AddItemToCartCommandHandlerTests()
        {
            _addItemToCartCommandHandler = new AddItemToCartCommandHandler(_cartRepository.Object);
        }

        [Fact]
        public async Task WhenCartWithGivenCartIdDoesNotExistThenThrowNotFoundException()
        {
            var cartId = Guid.NewGuid();
            var inputCommand = CreateCommand(cartId, Guid.NewGuid(), 100);
            _cartRepository.Setup(r => r.GetById(cartId))
                .ReturnsAsync(() => null);

            Func<Task> act = async () =>
                await _addItemToCartCommandHandler.Handle(inputCommand, new CancellationToken(false));

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task WhenItemWithGivenProductIdIsNotInCartThenAddItemToCart()
        {
            var cartId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var inputCommand = CreateCommand(cartId, productId, 100);
            var cartReturnedFromRepository = CreateEmptyCart(cartId);
            CartItem itemOnUpdate = null;
            CartEntity cartOnUpdate = null;
            _cartRepository.Setup(r => r.GetById(cartId))
                .ReturnsAsync(cartReturnedFromRepository);
            _cartRepository.Setup(r => r.UpdateWithAddedItem(It.IsAny<CartEntity>(), It.IsAny<CartItem>()))
                .Callback<CartEntity, CartItem>((c, i) =>
                {
                    cartOnUpdate = c;
                    itemOnUpdate = i;
                });

            await _addItemToCartCommandHandler.Handle(inputCommand, new CancellationToken(false));

            itemOnUpdate.ProductId.Should().Be(productId);
            itemOnUpdate.Quantity.Should().Be(100);
            cartOnUpdate.Items.Count.Should().Be(1);
            cartOnUpdate.Items[0].ProductId.Should().Be(productId);
            cartOnUpdate.Items[0].Quantity.Should().Be(100);
        }

        [Fact]
        public async Task WhenItemWithGivenProductIdAlreadyInCartThenAddQuantityToThisItem()
        {
            var cartId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var inputCommand = CreateCommand(cartId, productId, 50);
            var cartReturnedFromRepository = CreateCartWithItem(cartId, CreateItem(productId, 100));
            CartEntity cartOnUpdate = null;
            _cartRepository.Setup(r => r.GetById(cartId))
                .ReturnsAsync(cartReturnedFromRepository);
            _cartRepository.Setup(r => r.Update(It.IsAny<CartEntity>()))
                .Callback<CartEntity>(c => cartOnUpdate = c);

            await _addItemToCartCommandHandler.Handle(inputCommand, new CancellationToken(false));

            cartOnUpdate.Items.Count.Should().Be(1);
            cartOnUpdate.Items[0].ProductId.Should().Be(productId);
            cartOnUpdate.Items[0].Quantity.Should().Be(150);
        }

        public AddItemToCartCommand CreateCommand(Guid cartId, Guid productId, int quantity)
        {
            return new AddItemToCartCommand
            {
                CartId = cartId,
                ProductId = productId,
                Quantity = quantity
            };
        }

        public CartEntity CreateEmptyCart(Guid cartId)
        {
            return new CartEntity
            {
                CartId = cartId,
            };
        }

        public CartEntity CreateCartWithItem(Guid cartId, CartItem item)
        {
            var cart = new CartEntity
            {
                CartId = cartId,
            };
            cart.Items.Add(item);

            return cart;
        }

        public CartItem CreateItem(Guid productId, int quantity)
        {
            return new CartItem
            {
                ProductId = productId,
                Quantity = quantity,
            };
        }
    }
}
