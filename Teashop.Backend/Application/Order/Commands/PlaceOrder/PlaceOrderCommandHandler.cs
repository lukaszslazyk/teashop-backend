using MediatR;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Teashop.Backend.Application.Order.Repositories;
using Teashop.Backend.Domain.Order.Entities;
using Teashop.Backend.Application.Product.Queries.GetProductsByMultipleIds;
using System;
using Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Application.Order.Commands.PlaceOrder
{
    public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, PlaceOrderCommandResult>
    {
        private readonly IMediator _mediator;
        private readonly IOrderRepository _orderRepository;
        private readonly IShippingMethodRepository _shippingMethodRepository;
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        private PlaceOrderCommand _request;
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
            SetupOperation(request);
            CreateOrder();
            await CalculatePrices();
            await SaveOrder();

            return GetResult();
        }

        private void SetupOperation(PlaceOrderCommand request)
        {
            _request = request;
        }

        private void CreateOrder()
        {
            _order = new OrderEntity
            {
                ContactInfo = _request.ContactInfo,
                ShippingAddress = _request.ShippingAddress,
                BillingAddress = _request.BillingAddress,
                ChosenShippingMethodName = _request.ChosenShippingMethodName,
                ChosenPaymentMethodName = _request.ChosenPaymentMethodName,
                PaymentCard = _request.PaymentCard,
            };
            _order.OrderLines.AddRange(MapOrderLines(_request.OrderLines));
        }

        private List<OrderLine> MapOrderLines(List<PlaceOrderCommandOrderLine> orderLines)
        {
            return orderLines
                .Select((line, index) => new OrderLine
                {
                    OrderLineNo = index,
                    ProductId = line.ProductId,
                    Quantity = line.Quantity
                })
                .ToList();
        }

        private async Task CalculatePrices()
        {
            await LoadDataNecessaryForPriceCalculation();
            _order.CalculatePrices();
        }

        private async Task LoadDataNecessaryForPriceCalculation()
        {
            await LoadShippingMethod();
            await LoadPaymentMethod();
            await LoadOrderLineProducts();
        }

        private async Task LoadPaymentMethod()
        {
            _order.ChosenPaymentMethod = await _paymentMethodRepository.GetByName(_order.ChosenPaymentMethodName);
        }

        private async Task LoadShippingMethod()
        {
            _order.ChosenShippingMethod = await _shippingMethodRepository.GetByName(_order.ChosenShippingMethodName);
        }

        private async Task LoadOrderLineProducts()
        {
            var products = await _mediator.Send(new GetProductsByMultipleIdsQuery { ProductIds = GetOrderLineProductIds() });
            _order.OrderLines
                .ForEach(line => line.Product = FindProductWithId(line.ProductId, products));
        }

        private List<Guid> GetOrderLineProductIds()
        {
            return _request.OrderLines
                .Select(line => line.ProductId)
                .ToList();
        }

        private ProductEntity FindProductWithId(Guid productId, List<ProductEntity> products)
        {
            return products
                .Where(p => p.ProductId == productId)
                .FirstOrDefault();
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
                OrderNumber = _order.OrderNumber,
            };
        }
    }
}
