using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Teashop.Backend.Application.Product.Queries.GetAllProducts;
using Teashop.Backend.Application.Product.Queries.GetProductById;
using Teashop.Backend.Application.Product.Queries.GetProductsInCategory;
using Teashop.Backend.Application.Product.Queries.GetProductsWithSearchPhrase;
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

            return Ok(_mapper.MapToMultiplePresentationals(products));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery() { ProductId = id });

            return Ok(_mapper.MapToPresentational(product));
        }

        [HttpGet("categories/{categoryName}")]
        public async Task<IActionResult> GetProductsInCategory(
            string categoryName,
            [FromQuery(Name = "pageIndex")] int? pageIndex,
            [FromQuery(Name = "pageSize")] int? pageSize)
        {
            var query = new GetProductsInCategoryQuery
            {
                CategoryName = categoryName,
                PageIndexQueried = pageIndex.HasValue,
                PageIndex = pageIndex ?? 0,
                PageSizeQueried = pageSize.HasValue,
                PageSize = pageSize ?? 0,
            };
            var result = await _mediator.Send(query);

            return Ok(_mapper.MapToResponse(result));
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetProductsWithSearchPhrase(
            [FromQuery(Name = "phrase")] string phrase,
            [FromQuery(Name = "pageIndex")] int? pageIndex,
            [FromQuery(Name = "pageSize")] int? pageSize)
        {
            var query = new GetProductsWithSearchPhraseQuery
            {
                Phrase = phrase,
                PageIndexQueried = pageIndex.HasValue,
                PageIndex = pageIndex ?? 0,
                PageSizeQueried = pageSize.HasValue,
                PageSize = pageSize ?? 0,
            };
            var result = await _mediator.Send(query);

            return Ok(_mapper.MapToResponse(result));
        }
    }
}
