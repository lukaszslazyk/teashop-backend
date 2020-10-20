using System.Collections.Generic;
using System.Threading.Tasks;
using Entities = Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Application.Product.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Entities.Product>> GetProducts();
    }
}
