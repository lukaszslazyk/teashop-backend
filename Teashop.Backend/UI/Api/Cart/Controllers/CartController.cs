﻿using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Teashop.Backend.Application.Cart.Commands;
using Teashop.Backend.Application.Cart.Commands.AddItemToCart;
using Teashop.Backend.Application.Cart.Commands.RemoveItemFromCart;
using Teashop.Backend.Application.Cart.Commands.UpdateItemQuantity;
using Teashop.Backend.Domain.Cart.Entities;
using Teashop.Backend.UI.Api.Cart.Mappings;
using Teashop.Backend.UI.Api.Cart.Models;
using Teashop.Backend.UI.Api.Cart.Utils;

namespace Teashop.Backend.UI.Api.Cart.Controllers
{
    [Route("api/carts")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly CartMapper _mapper;
        private readonly SessionCartHandler _sessionCartHandler;

        public CartController(
            IMediator mediator,
            CartMapper mapper,
            SessionCartHandler sessionCartHandler)
        {
            _mediator = mediator;
            _mapper = mapper;
            _sessionCartHandler = sessionCartHandler;
        }

        [HttpGet("sessionCart")]
        public async Task<IActionResult> GetSessionCart()
        {
            await EnsureSessionHasCart();
            var cart = await GetCartById(GetSessionCartId());

            return Ok(_mapper.MapToPresentational(cart));
        }

        [HttpPost("sessionCart/items")]
        public async Task<IActionResult> AddItemToSessionCart(AddItemToSessionCartRequest request)
        {
            await EnsureSessionHasCart();
            await AddItemToCart(GetSessionCartId(), request.ProductId, request.Quantity);

            return Ok();
        }

        [HttpPatch("sessionCart/items")]
        public async Task<IActionResult> UpdateSessionCartItemQuantity(
            UpdateSessionCartItemQuantityRequest request)
        {
            await EnsureSessionHasCart();
            await UpdateItemQuantity(GetSessionCartId(), request.ProductId, request.Quantity);

            return Ok();
        }

        [HttpDelete("sessionCart/items")]
        public async Task<IActionResult> RemoveItemFromSessionCart(RemoveItemFromSessionCartRequest request)
        {
            await EnsureSessionHasCart();
            await RemoveItemFromCart(GetSessionCartId(), request.ProductId);

            return Ok();
        }

        private async Task EnsureSessionHasCart()
        {
            await _sessionCartHandler.EnsureSessionHasCart(HttpContext.Session);
        }
        public Guid GetSessionCartId()
        {
            return _sessionCartHandler.GetSessionCartId(HttpContext.Session);
        }

        private async Task<CartEntity> GetCartById(Guid cartId)
        {
            return await _mediator.Send(new GetCartByIdQuery { CartId = cartId });
        }

        private async Task AddItemToCart(Guid cartId, Guid productId, int quantity)
        {
            var command = new AddItemToCartCommand
            {
                CartId = cartId,
                ProductId = productId,
                Quantity = quantity
            };
            await _mediator.Send(command);
        }

        private async Task UpdateItemQuantity(Guid cartId, Guid productId, int quantity)
        {
            var command = new UpdateItemQuantityCommand
            {
                CartId = cartId,
                ProductId = productId,
                Quantity = quantity
            };
            await _mediator.Send(command);
        }

        private async Task RemoveItemFromCart(Guid cartId, Guid productId)
        {
            var command = new RemoveItemFromCartCommand
            {
                CartId = cartId,
                ProductId = productId
            };
            await _mediator.Send(command);
        }
    }
}