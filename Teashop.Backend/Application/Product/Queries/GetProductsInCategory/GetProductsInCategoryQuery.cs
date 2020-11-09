using MediatR;
using System.Collections.Generic;
using Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Application.Product.Queries.GetProductsInCategory
{
    public class GetProductsInCategoryQuery : IRequest<IEnumerable<ProductEntity>>
    {
        public string CategoryName { get; set; }
    }
}
