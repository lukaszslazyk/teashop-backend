using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Teashop.Backend.Application.Product.Commands.GetAllProducts;
using Teashop.Backend.UI.Api.Product.Mappings;
using Teashop.Backend.UI.Api.Product.Models;

namespace Teashop.Backend.UI.Api.Product.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _mediator.Send(new GetAllProductsQuery());

            var productsResponse = products
                .Select(product => new PresentationalProduct().From(product));

            return Ok(productsResponse);
        }
    }
}
