using MediatR;
using Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Application.Product.Queries.GetProductByProductNumber
{
    public class GetProductByProductNumberQuery : IRequest<ProductEntity>
    {
        public int ProductNumber { get; set; }
    }
}
