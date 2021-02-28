using FluentAssertions;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Teashop.Backend.Application.Commons.Exceptions;
using Teashop.Backend.Application.Order.Commands.PlaceOrder;
using Teashop.Backend.Application.Order.Repositories;
using Teashop.Backend.Application.Product.Queries.GetProductsByMultipleIds;
using Teashop.Backend.Domain.Order.Entities;
using Teashop.Backend.Domain.Product.Entities;
using Xunit;

namespace Teashop.Backend.Tests.UnitTests.Application.Order.Command.PlaceOrder
{
    public class PlaceOrderCommandHandlerTests
    {
        private readonly PlaceOrderCommandHandler _placeOrderCommandHandler;
        private readonly Mock<IMediator> _mediator = new Mock<IMediator>();
        private readonly Mock<IOrderRepository> _orderRepository = new Mock<IOrderRepository>();
        private readonly Mock<IShippingMethodRepository> _shippingMethodRepository = new Mock<IShippingMethodRepository>();
        private readonly Mock<IPaymentMethodRepository> _paymentMethodRepository = new Mock<IPaymentMethodRepository>();

        public PlaceOrderCommandHandlerTests()
        {
            _placeOrderCommandHandler = new PlaceOrderCommandHandler(
                _mediator.Object,
                _orderRepository.Object,
                _shippingMethodRepository.Object,
                _paymentMethodRepository.Object);
        }

        [Fact]
        public async Task WhenPlacingOrderThenLoadDataNecessaryForPriceCalculation()
        {
            var productId1 = Guid.NewGuid();
            var productId2 = Guid.NewGuid();
            var inputCommand = new PlaceOrderCommand
            {
                ChosenShippingMethodName = "shipping1",
                ChosenPaymentMethodName = "payment1",
                OrderLines = GetPlaceOrderCommandOrderLinesWithProductIds(productId1, productId2)
            };
            OrderEntity orderOnRepositoryInput = null;
            _mediator.Setup(m => m.Send(It.IsAny<GetProductsByMultipleIdsQuery>(), default))
                .ReturnsAsync(GetProductsWithIds(productId2, productId1));
            _shippingMethodRepository.Setup(r => r.GetByName("shipping1"))
                .ReturnsAsync(new ShippingMethod { Fee = 1.0 });
            _paymentMethodRepository.Setup(r => r.GetByName("payment1"))
                .ReturnsAsync(new PaymentMethod { Fee = 1.0 });
            _orderRepository.Setup(r => r.Create(It.IsAny<OrderEntity>()))
                .Callback<OrderEntity>(o => orderOnRepositoryInput = o);

            var result = await _placeOrderCommandHandler.Handle(inputCommand, default);

            orderOnRepositoryInput.ChosenShippingMethod.Should().NotBeNull();
            orderOnRepositoryInput.ChosenPaymentMethod.Should().NotBeNull();
            orderOnRepositoryInput.OrderLines.Count.Should().Be(2);
            orderOnRepositoryInput.OrderLines[0].Product.ProductId.Should().Be(productId1);
            orderOnRepositoryInput.OrderLines[1].Product.ProductId.Should().Be(productId2);
            orderOnRepositoryInput.TotalPrice.Should().BeGreaterThan(0);
            orderOnRepositoryInput.SubtotalPrice.Should().BeGreaterThan(0);
            orderOnRepositoryInput.ShippingFee.Should().BeGreaterThan(0);
            orderOnRepositoryInput.PaymentFee.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task WhenGetProductsByMultipleIdsQueryThrowsNotFoundExceptionThenPassItFurther()
        {
            var productId1 = Guid.NewGuid();
            var productId2 = Guid.NewGuid();
            var inputCommand = new PlaceOrderCommand
            {
                ChosenShippingMethodName = "shipping1",
                ChosenPaymentMethodName = "payment1",
                OrderLines = GetPlaceOrderCommandOrderLinesWithProductIds(productId1, productId2)
            };
            _mediator.Setup(m => m.Send(It.IsAny<GetProductsByMultipleIdsQuery>(), default))
                .ThrowsAsync(new NotFoundException("foo"));

            Func<Task> act = async () =>
                await _placeOrderCommandHandler.Handle(inputCommand, default);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("foo");
        }

        [Fact]
        public async Task WhenPlacingOrderThenAssignOrderingNumbersToOrderLines()
        {
            var productId1 = Guid.NewGuid();
            var productId2 = Guid.NewGuid();
            var inputCommand = new PlaceOrderCommand
            {
                ChosenShippingMethodName = "shipping1",
                ChosenPaymentMethodName = "payment1",
                OrderLines = GetPlaceOrderCommandOrderLinesWithProductIds(productId1, productId2)
            };
            OrderEntity orderOnRepositoryInput = null;
            _mediator.Setup(m => m.Send(It.IsAny<GetProductsByMultipleIdsQuery>(), default))
                .ReturnsAsync(GetProductsWithIds(productId2, productId1));
            _shippingMethodRepository.Setup(r => r.GetByName("shipping1"))
                .ReturnsAsync(new ShippingMethod { Fee = 1.0 });
            _paymentMethodRepository.Setup(r => r.GetByName("payment1"))
                .ReturnsAsync(new PaymentMethod { Fee = 1.0 });
            _orderRepository.Setup(r => r.Create(It.IsAny<OrderEntity>()))
                .Callback<OrderEntity>(o => orderOnRepositoryInput = o);

            var result = await _placeOrderCommandHandler.Handle(inputCommand, default);

            orderOnRepositoryInput.OrderLines.Count.Should().Be(2);
            orderOnRepositoryInput.OrderLines[0].OrderLineNo.Should().Be(0);
            orderOnRepositoryInput.OrderLines[1].OrderLineNo.Should().Be(1);
        }

        [Fact]
        public async Task WhenOrderPlacedThenReturnResultWithOrderIdAndOrderNumber()
        {
            var productId1 = Guid.NewGuid();
            var productId2 = Guid.NewGuid();
            var inputCommand = new PlaceOrderCommand
            {
                ChosenShippingMethodName = "shipping1",
                ChosenPaymentMethodName = "payment1",
                OrderLines = GetPlaceOrderCommandOrderLinesWithProductIds(productId1, productId2)
            };
            OrderEntity orderOnRepositoryInput = null;
            var createdOrderId = Guid.NewGuid();
            var createdOrderNumber = 100001;
            _mediator.Setup(m => m.Send(It.IsAny<GetProductsByMultipleIdsQuery>(), default))
                .ReturnsAsync(GetProductsWithIds(productId2, productId1));
            _shippingMethodRepository.Setup(r => r.GetByName("shipping1"))
                .ReturnsAsync(new ShippingMethod { Fee = 1.0 });
            _paymentMethodRepository.Setup(r => r.GetByName("payment1"))
                .ReturnsAsync(new PaymentMethod { Fee = 1.0 });
            _orderRepository.Setup(r => r.Create(It.IsAny<OrderEntity>()))
                .Callback<OrderEntity>(o =>
                {
                    orderOnRepositoryInput = o;
                    orderOnRepositoryInput.OrderId = createdOrderId;
                    orderOnRepositoryInput.OrderNumber = createdOrderNumber;
                });

            var result = await _placeOrderCommandHandler.Handle(inputCommand, default);

            result.OrderId.Should().Be(createdOrderId);
            result.OrderNumber.Should().Be(createdOrderNumber);
        }

        private List<PlaceOrderCommandOrderLine> GetPlaceOrderCommandOrderLinesWithProductIds(Guid productId1, Guid productId2)
        {
            return new List<PlaceOrderCommandOrderLine>
            {
                new PlaceOrderCommandOrderLine
                {
                    ProductId = productId1,
                    Quantity = 50,
                },
                new PlaceOrderCommandOrderLine
                {
                    ProductId = productId2,
                    Quantity = 100,
                },
            };
        }

        private List<ProductEntity> GetProductsWithIds(Guid productId1, Guid productId2)
        {
            return new List<ProductEntity>
            {
                new ProductEntity
                {
                    ProductId = productId1,
                    Price = 9.99,
                    QuantityPerPrice = 100
                },
                new ProductEntity
                {
                    ProductId = productId2,
                    Price = 19.99,
                    QuantityPerPrice = 100
                },
            };
        }
    }
}
