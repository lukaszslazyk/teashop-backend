using MediatR;
using System;
using System.Collections.Generic;
using Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Application.Product.Queries.GetProductsByMultipleIds
{
    public class GetProductsByMultipleIdsQuery : IRequest<List<ProductEntity>>
    {
        public List<Guid> ProductIds { get; set; }
    }
}
