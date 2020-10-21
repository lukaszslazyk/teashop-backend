using MediatR;
using System.Collections.Generic;
using Entities = Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Application.Product.Commands.GetAllProducts
{
    public class GetAllProductsQuery : IRequest<IEnumerable<Entities.Product>>
    { }
}
