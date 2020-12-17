using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Teashop.Backend.Application.Order.Repositories;
using Teashop.Backend.Domain.Order.Entities;

namespace Teashop.Backend.Application.Order.Commands.PlaceOrder
{
    public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, PlaceOrderCommandResult>
    {
        private readonly IOrderRepository _orderRepository;
        private OrderEntity _order;

        public PlaceOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<PlaceOrderCommandResult> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            CreateOrderFrom(request);
            await SaveOrder();

            return GetResult();
        }

        private void CreateOrderFrom(PlaceOrderCommand request)
        {
            _order = new OrderEntity
            {
                ContactInfo = request.ContactInfo,
                ShippingAddress = request.ShippingAddress,
                BillingAddress = request.BillingAddress,
                ChosenShippingMethodName = request.ChosenShippingMethodName,
                ChosenPaymentMethodName = request.ChosenPaymentMethodName,
                PaymentCard = request.PaymentCard,
                CartId = request.CartId,
            };
        }

        private async Task SaveOrder()
        {
            await _orderRepository.Create(_order);
        }

        private PlaceOrderCommandResult GetResult()
        {
            return new PlaceOrderCommandResult
            {
                OrderId = _order.OrderId,
                OrderNo = _order.OrderNo,
            };
        }
    }
}
