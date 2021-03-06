using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Teashop.Backend.Application.Order.Commands.PlaceOrder;
using Teashop.Backend.Application.Order.Queries.GetOrderById;
using Teashop.Backend.Application.Order.Queries.GetOrderMeta;
using Teashop.Backend.UI.Api.Commons.Session;
using Teashop.Backend.UI.Api.Order.Mappings;
using Teashop.Backend.UI.Api.Order.Models;

namespace Teashop.Backend.UI.Api.Order.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly SessionHandler _sessionHandler;
        private readonly OrderMapper _orderMapper;
        private readonly OrderMetaMapper _orderMetaMapper;

        public OrderController(
            IMediator mediator,
            SessionHandler sessionHandler,
            OrderMapper orderMapper,
            OrderMetaMapper orderMetaMapper)
        {
            _mediator = mediator;
            _sessionHandler = sessionHandler;
            _orderMapper = orderMapper;
            _orderMetaMapper = orderMetaMapper;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(PlaceOrderRequest request)
        {
            var result = await PlaceOrderFrom(request);
            CloseSessionCart();

            return Ok(_orderMapper.MapToResponse(result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var order = await _mediator.Send(new GetOrderByIdQuery() { OrderId = id });

            return Ok(_orderMapper.MapToPresentational(order));
        }

        [HttpGet("meta")]
        public async Task<IActionResult> GetOrderMeta()
        {
            var orderMeta = await _mediator.Send(new GetOrderMetaQuery());

            return Ok(_orderMetaMapper.MapToPresentational(orderMeta));
        }

        private async Task<PlaceOrderCommandResult> PlaceOrderFrom(PlaceOrderRequest request)
        {
            return await _mediator.Send(GetPlaceOrderCommand(request));
        }

        private PlaceOrderCommand GetPlaceOrderCommand(PlaceOrderRequest request)
        {
            return new PlaceOrderCommand
            {
                ContactInfo = _orderMapper.MapFromRequest(request.ContactInfo),
                ShippingAddress = _orderMapper.MapFromRequest(request.ShippingAddress),
                BillingAddress = _orderMapper.MapFromRequest(request.BillingAddress),
                ChosenShippingMethodName = request.ChosenShippingMethodName,
                ChosenPaymentMethodName = request.ChosenPaymentMethodName,
                PaymentCard = _orderMapper.MapFromRequest(request.PaymentCard),
                OrderLines = _orderMapper.MapFromRequest(request.OrderLines)
            };
        }

        private void CloseSessionCart()
        {
            _sessionHandler.CloseSessionCart(HttpContext.Session);
        }
    }
}
