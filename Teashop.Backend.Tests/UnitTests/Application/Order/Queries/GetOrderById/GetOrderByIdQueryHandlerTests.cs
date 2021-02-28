using FluentAssertions;
using Moq;
using System;
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
                await _getOrderByIdQueryHandler.Handle(inputQuery, default);

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
                .Handle(inputQuery, default);

            orderReturned.OrderId.Should().Be(orderId);
        }

        [Fact]
        public async Task WhenOrderLinesReturnedFromRepositoryAreUnsortedThenReturnOrderWithOrderLinesSortedByOrderLineNo()
        {
            var orderId = Guid.NewGuid();
            var inputQuery = CreateQuery(orderId);
            var returnedFromRepository = CreateOrder(orderId);
            returnedFromRepository.OrderLines.Add(CreateOrderLine(2));
            returnedFromRepository.OrderLines.Add(CreateOrderLine(0));
            returnedFromRepository.OrderLines.Add(CreateOrderLine(1));
            _orderRepository.Setup(r => r.GetById(orderId))
                .ReturnsAsync(returnedFromRepository);

            var orderReturned = await _getOrderByIdQueryHandler
                .Handle(inputQuery, default);

            orderReturned.OrderLines.Count.Should().Be(3);
            orderReturned.OrderLines[0].OrderLineNo.Should().Be(0);
            orderReturned.OrderLines[1].OrderLineNo.Should().Be(1);
            orderReturned.OrderLines[2].OrderLineNo.Should().Be(2);
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

        private OrderLine CreateOrderLine(int orderLineNo)
        {
            return new OrderLine
            {
                OrderLineNo = orderLineNo
            };
        }
    }
}
