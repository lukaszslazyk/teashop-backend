using MediatR;
using System;
using Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Application.Product.Queries.GetProductById
{
    public class GetProductByIdQuery : IRequest<ProductEntity>
    {
        public Guid ProductId { get; set; }
    }
}
