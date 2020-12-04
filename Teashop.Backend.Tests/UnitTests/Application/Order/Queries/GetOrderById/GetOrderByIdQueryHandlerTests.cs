using FluentAssertions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Teashop.Backend.Application.Commons.Exceptions;
using Teashop.Backend.Application.Order.Queries.GetOrderById;
using Teashop.Backend.Application.Order.Repositories;
using Teashop.Backend.Domain.Order.Entities;
using Xunit;

namespace Teashop.Backend.Tests.UnitTests.Application.Order.Queries.GetOrderById
{
    public class GetOrderByIdQueryHandlerTests
    {
        private readonly GetOrderByIdQueryHandler _getOrderByIdQueryHandler;
        private readonly Mock<IOrderRepository> _orderRepository = new Mock<IOrderRepository>();

        public GetOrderByIdQueryHandlerTests()
        {
            _getOrderByIdQueryHandler = new GetOrderByIdQueryHandler(_orderRepository.Object);
        }

        [Fact]
        public async Task WhenOrderWithGivenIdDoesNotExistThenThrowNotFoundException()
        {
            var orderId = Guid.NewGuid();
            var inputQuery = CreateQuery(orderId);
            _orderRepository.Setup(r => r.GetById(orderId))
                .ReturnsAsync(() => null);

            Func<Task> act = async () =>
                await _getOrderByIdQueryHandler.Handle(inputQuery, new CancellationToken(false));

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task WhenOrderWithGivenIdExistsThenReturnOrder()
        {
            var orderId = Guid.NewGuid();
            var inputQuery = CreateQuery(orderId);
            _orderRepository.Setup(r => r.GetById(orderId))
                .ReturnsAsync(CreateOrder(orderId));

            var orderReturned = await _getOrderByIdQueryHandler
                .Handle(inputQuery, new CancellationToken(false));

            orderReturned.OrderId.Should().Be(orderId);
        }

        private GetOrderByIdQuery CreateQuery(Guid orderId)
        {
            return new GetOrderByIdQuery
            {
                OrderId = orderId
            };
        }

        private OrderEntity CreateOrder(Guid orderId)
        {
            return new OrderEntity
            {
                OrderId = orderId,
            };
        }
    }
}
