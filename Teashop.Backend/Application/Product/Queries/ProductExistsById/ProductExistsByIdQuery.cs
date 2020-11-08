using MediatR;
using System;

namespace Teashop.Backend.Application.Product.Queries.ProductExistsById
{
    public class ProductExistsByIdQuery : IRequest<bool>
    {
        public Guid ProductId { get; set; }
    }
}
