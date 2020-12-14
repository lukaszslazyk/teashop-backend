using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Teashop.Backend.Application.Commons.Exceptions;
using Teashop.Backend.Application.Order.Repositories;
using Teashop.Backend.Domain.Order.Entities;

namespace Teashop.Backend.Application.Order.Queries.GetOrderById
{
    public class GetOrderByOrderNoQueryHandler : IRequestHandler<GetOrderByOrderNo, OrderEntity>
    {
        private readonly IOrderRepository _orderRepository;
        private OrderEntity _order;

        public GetOrderByOrderNoQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderEntity> Handle(GetOrderByOrderNo request, CancellationToken cancellationToken)
        {
            await LoadOrderWith(request.OrderNo);
            if (!OrderFound())
                ThrowNotFoundException();

            return _order;
        }

        private async Task LoadOrderWith(int orderNo)
        {
            _order = await _orderRepository.GetByOrderNo(orderNo);
        }

        private bool OrderFound()
        {
            return _order != null;
        }

        private void ThrowNotFoundException()
        {
            throw new NotFoundException("Order with given number does not exist.");
        }
    }
}
