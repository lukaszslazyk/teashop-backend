using MediatR;

namespace Teashop.Backend.Application.Product.Queries.GetProductsInCategory
{
    public class GetProductsInCategoryQuery : IRequest<GetProductsInCategoryQueryResult>
    {
        public string CategoryName { get; set; }
        public bool pageIndexQueried { get; set; }
        public int pageIndex { get; set; }
        public bool pageSizeQueried { get; set; }
        public int pageSize { get; set; }
    }
}
