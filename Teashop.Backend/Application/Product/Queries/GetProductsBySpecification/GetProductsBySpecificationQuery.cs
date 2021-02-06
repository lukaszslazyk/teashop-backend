using MediatR;
using Teashop.Backend.Application.Commons.Models;
using Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Application.Product.Queries.GetProductsBySpecification
{
    public class GetProductsBySpecificationQuery : IRequest<PaginatedList<ProductEntity>>
    {
        public ProductsQuerySpecification Specification { get; set; }
    }
}
