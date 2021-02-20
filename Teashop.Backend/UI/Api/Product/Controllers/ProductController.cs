using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Teashop.Backend.Application.Product.Queries.GetProductById;
using Teashop.Backend.Application.Product.Queries.GetProductByProductNumber;
using Teashop.Backend.Application.Product.Queries.GetProductsBySpecification;
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
        public async Task<IActionResult> GetProductsBySpecification(
            [FromQuery(Name = "categoryName")] string categoryName,
            [FromQuery(Name = "searchPhrase")] string searchPhrase,
            [FromQuery(Name = "orderBy")] string orderBy,
            [FromQuery(Name = "pageIndex")] int? pageIndex,
            [FromQuery(Name = "pageSize")] int? pageSize)
        {
            var query = new GetProductsBySpecificationQuery
            {
                Specification = new ProductsQuerySpecification
                {
                    CategoryNameQueried = categoryName != null,
                    CategoryName = categoryName,
                    SearchPhraseQueried = searchPhrase != null,
                    SearchPhrase = searchPhrase,
                    OrderByQueried = orderBy != null,
                    OrderBy = orderBy,
                    PageIndexQueried = pageIndex.HasValue,
                    PageIndex = pageIndex ?? 0,
                    PageSizeQueried = pageSize.HasValue,
                    PageSize = pageSize ?? 0,
                }
            };
            var result = await _mediator.Send(query);

            return Ok(_mapper.MapToResponse(result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery { ProductId = id });

            return Ok(_mapper.MapToPresentational(product));
        }

        [HttpGet("number/{productNumber}")]
        public async Task<IActionResult> GetProductByProductNumber(int productNumber)
        {
            var product = await _mediator.Send(new GetProductByProductNumberQuery { ProductNumber = productNumber });

            return Ok(_mapper.MapToPresentational(product));
        }
    }
}
