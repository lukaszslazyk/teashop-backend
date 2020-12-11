using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Teashop.Backend.Application.Order.Commands.PlaceOrder;
using Teashop.Backend.Application.Order.Queries.GetOrderById;
using Teashop.Backend.Application.Order.Queries.GetOrderMeta;
using Teashop.Backend.UI.Api.Cart.Utils;
using Teashop.Backend.UI.Api.Commons.Exceptions;
using Teashop.Backend.UI.Api.Order.Exceptions;
using Teashop.Backend.UI.Api.Order.Mappings;
using Teashop.Backend.UI.Api.Order.Models;

namespace Teashop.Backend.UI.Api.Order.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly SessionCartHandler _sessionCartHandler;
        private readonly OrderMapper _orderMapper;
        private readonly OrderMetaMapper _orderMetaMapper;

        public OrderController(
            IMediator mediator,
            SessionCartHandler sessionCartHandler,
            OrderMapper orderMapper,
            OrderMetaMapper orderMetaMapper)
        {
            _mediator = mediator;
            _sessionCartHandler = sessionCartHandler;
            _orderMapper = orderMapper;
            _orderMetaMapper = orderMetaMapper;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(PlaceOrderRequest request)
        {
            var orderId = await PlaceOrderWithSessionCart(request);
            RemoveCartFromSession();

            return Ok(orderId);
        }


        [HttpGet("{orderNo}")]
        public async Task<IActionResult> GetOrderByOrderNo(int orderNo)
        {
            var order = await _mediator.Send(new GetOrderByOrderNo() { OrderNo = orderNo });

            return Ok(_orderMapper.MapToPresentational(order));
        }

        [HttpGet("meta")]
        public async Task<IActionResult> GetOrderMeta()
        {
            var orderMeta = await _mediator.Send(new GetOrderMetaQuery());

            return Ok(_orderMetaMapper.MapToPresentational(orderMeta));
        }

        private async Task<int> PlaceOrderWithSessionCart(PlaceOrderRequest request)
        {
            return await _mediator.Send(GetPlaceOrderCommand(request, GetSessionCartId()));
        }

        private Guid GetSessionCartId()
        {
            try
            {
                return _sessionCartHandler.GetSessionCartId(HttpContext.Session);
            }
            catch (SessionCartIdNotSetException)
            {
                throw new SessionCartNotCreatedException();
            }
        }

        private void RemoveCartFromSession()
        {
            _sessionCartHandler.RemoveCartFromSession(HttpContext.Session);
        }

        private PlaceOrderCommand GetPlaceOrderCommand(PlaceOrderRequest request, Guid cartId)
        {
            return new PlaceOrderCommand
            {
                ContactInfo = _orderMapper.MapFromRequest(request.ContactInfo),
                ShippingAddress = _orderMapper.MapFromRequest(request.ShippingAddress),
                ChosenShippingMethodName = request.ChosenShippingMethodName,
                ChosenPaymentMethodName = request.ChosenPaymentMethodName,
                PaymentCard = _orderMapper.MapFromRequest(request.PaymentCard),
                CartId = cartId,
            };
        }
    }
}
