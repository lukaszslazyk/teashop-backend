using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Teashop.Backend.Application.Order.Queries.GetOrderMeta;
using Teashop.Backend.UI.Api.Order.Mappings;

namespace Teashop.Backend.UI.Api.Order.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly OrderMetaMapper _orderMetaMapper;

        public OrderController(IMediator mediator, OrderMetaMapper orderMetaMapper)
        {
            _mediator = mediator;
            _orderMetaMapper = orderMetaMapper;
        }

        [HttpGet("meta")]
        public async Task<IActionResult> GetOrderMeta()
        {
            var orderMeta = await _mediator.Send(new GetOrderMetaQuery());

            return Ok(_orderMetaMapper.MapToPresentational(orderMeta));
        }
    }
}
