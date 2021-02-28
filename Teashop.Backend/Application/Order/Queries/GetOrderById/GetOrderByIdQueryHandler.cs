using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Teashop.Backend.Application.Commons.Exceptions;
using Teashop.Backend.Application.Order.Repositories;
using Teashop.Backend.Domain.Order.Entities;

namespace Teashop.Backend.Application.Order.Queries.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderEntity>
    {
        private readonly IOrderRepository _orderRepository;
        private OrderEntity _order;

        public GetOrderByIdQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderEntity> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            await LoadOrderWith(request.OrderId);
            if (!OrderFound())
                ThrowNotFoundException();
            SortOrderLines();

            return _order;
        }

        private async Task LoadOrderWith(Guid orderId)
        {
            _order = await _orderRepository.GetById(orderId);
        }

        private bool OrderFound()
        {
            return _order != null;
        }

        private void ThrowNotFoundException()
        {
            throw new NotFoundException("Order with given id does not exist.");
        }

        private void SortOrderLines()
        {
            _order.OrderLines.Sort((x, y) => x.OrderLineNo.CompareTo(y.OrderLineNo));
        }
    }
}
