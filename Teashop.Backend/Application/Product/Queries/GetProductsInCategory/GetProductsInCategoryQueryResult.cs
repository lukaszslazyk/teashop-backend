using System.Collections.Generic;
using Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Application.Product.Queries.GetProductsInCategory
{
    public class GetProductsInCategoryQueryResult
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PagesInTotal { get; set; }
        public IEnumerable<ProductEntity> Products { get; set; }
    }
}
