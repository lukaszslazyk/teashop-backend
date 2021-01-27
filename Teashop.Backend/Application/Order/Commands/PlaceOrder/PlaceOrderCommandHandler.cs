using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Teashop.Backend.Application.Cart.Queries.GetCartById;
using Teashop.Backend.Application.Order.Repositories;
using Teashop.Backend.Domain.Order.Entities;

namespace Teashop.Backend.Application.Order.Commands.PlaceOrder
{
    public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, PlaceOrderCommandResult>
    {
        private readonly IMediator _mediator;
        private readonly IOrderRepository _orderRepository;
        private readonly IShippingMethodRepository _shippingMethodRepository;
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        private OrderEntity _order;

        public PlaceOrderCommandHandler(
            IMediator mediator,
            IOrderRepository orderRepository,
            IShippingMethodRepository shippingMethodRepository,
            IPaymentMethodRepository paymentMethodRepository)
        {
            _mediator = mediator;
            _orderRepository = orderRepository;
            _shippingMethodRepository = shippingMethodRepository;
            _paymentMethodRepository = paymentMethodRepository;
        }

        public async Task<PlaceOrderCommandResult> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            CreateOrderFrom(request);
            await CalculateOrderTotalPrice();
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

        private async Task CalculateOrderTotalPrice()
        {
            _order.Cart = await _mediator.Send(new GetCartByIdQuery() { CartId = _order.CartId });
            _order.ChosenShippingMethod = await _shippingMethodRepository.GetByName(_order.ChosenShippingMethodName);
            _order.ChosenPaymentMethod = await _paymentMethodRepository.GetByName(_order.ChosenPaymentMethodName);
            _order.CalculateTotalPrice();
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
