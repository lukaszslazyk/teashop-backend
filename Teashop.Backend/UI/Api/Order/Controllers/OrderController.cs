﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Teashop.Backend.Application.Order.Commands.PlaceOrder;
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
            var orderId = await _mediator.Send(GetPlaceOrderCommand(request, GetSessionCartId()));

            return Ok(orderId);
        }

        [HttpGet("meta")]
        public async Task<IActionResult> GetOrderMeta()
        {
            var orderMeta = await _mediator.Send(new GetOrderMetaQuery());

            return Ok(_orderMetaMapper.MapToPresentational(orderMeta));
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

        private PlaceOrderCommand GetPlaceOrderCommand(PlaceOrderRequest request, Guid cartId)
        {
            return new PlaceOrderCommand
            {
                ContactInfo = _orderMapper.MapFromPresentational(request.ContactInfo),
                ShippingAddress = _orderMapper.MapFromPresentational(request.ShippingAddress),
                ChosenShippingMethodName = request.ChosenShippingMethodName,
                ChosenPaymentMethodName = request.ChosenPaymentMethodName,
                PaymentCard = _orderMapper.MapFromPresentational(request.PaymentCard),
                CartId = cartId,
            };
        }
    }
}
