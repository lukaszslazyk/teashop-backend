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
    public class GetOrderByOrderNoQueryHandlerTests
    {
        private readonly GetOrderByOrderNoQueryHandler _getOrderByIdQueryHandler;
        private readonly Mock<IOrderRepository> _orderRepository = new Mock<IOrderRepository>();

        public GetOrderByOrderNoQueryHandlerTests()
        {
            _getOrderByIdQueryHandler = new GetOrderByOrderNoQueryHandler(_orderRepository.Object);
        }

        [Fact]
        public async Task WhenOrderWithGivenNumberDoesNotExistThenThrowNotFoundException()
        {
            var orderNo = 100000;
            var inputQuery = CreateQuery(orderNo);
            _orderRepository.Setup(r => r.GetByOrderNo(orderNo))
                .ReturnsAsync(() => null);

            Func<Task> act = async () =>
                await _getOrderByIdQueryHandler.Handle(inputQuery, new CancellationToken(false));

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task WhenOrderWithGivenNumberExistsThenReturnOrder()
        {
            var orderNo = 100000;
            var inputQuery = CreateQuery(orderNo);
            _orderRepository.Setup(r => r.GetByOrderNo(orderNo))
                .ReturnsAsync(CreateOrder(orderNo));

            var orderReturned = await _getOrderByIdQueryHandler
                .Handle(inputQuery, new CancellationToken(false));

            orderReturned.OrderNo.Should().Be(orderNo);
        }

        private GetOrderByOrderNo CreateQuery(int orderNo)
        {
            return new GetOrderByOrderNo
            {
                OrderNo = orderNo
            };
        }

        private OrderEntity CreateOrder(int orderNo)
        {
            return new OrderEntity
            {
                OrderNo = orderNo
            };
        }
    }
}
