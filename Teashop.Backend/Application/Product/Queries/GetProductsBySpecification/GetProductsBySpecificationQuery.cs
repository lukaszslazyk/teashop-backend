using MediatR;
using Teashop.Backend.Application.Commons.Models;
using Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Application.Product.Queries.GetProductsBySpecification
{
    public class GetProductsBySpecificationQuery : IRequest<PaginatedList<ProductEntity>>
    {
        public bool CategoryNameQueried { get; set; }
        public string CategoryName { get; set; }
        public bool SearchPhraseQueried { get; set; }
        public string SearchPhrase { get; set; }
        public bool OrderByQueried { get; set; }
        public string OrderBy { get; set; }
        public bool PageIndexQueried { get; set; }
        public int PageIndex { get; set; }
        public bool PageSizeQueried { get; set; }
        public int PageSize { get; set; }
    }
}
