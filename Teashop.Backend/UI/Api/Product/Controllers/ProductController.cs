using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Teashop.Backend.Application.Product.Queries.GetAllProducts;
using Teashop.Backend.UI.Api.Product.Mappings;

namespace Teashop.Backend.UI.Api.Product.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ProductMapper _mapper;

        public ProductController(IMediator mediator, ProductMapper productMapper)
        {
            _mediator = mediator;
            _mapper = productMapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _mediator.Send(new GetAllProductsQuery());

            return Ok(_mapper.MapToPresentationals(products));
        }
    }
}
