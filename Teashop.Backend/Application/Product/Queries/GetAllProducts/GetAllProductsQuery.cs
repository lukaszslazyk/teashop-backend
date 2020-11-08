using MediatR;
using System.Collections.Generic;
using Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Application.Product.Queries.GetAllProducts
{
    public class GetAllProductsQuery : IRequest<IEnumerable<ProductEntity>>
    { }
}
