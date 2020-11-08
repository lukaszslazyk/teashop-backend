using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Application.Product.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductEntity>> GetAll();
        Task<bool> ExistsById(Guid productId);
    }
}
