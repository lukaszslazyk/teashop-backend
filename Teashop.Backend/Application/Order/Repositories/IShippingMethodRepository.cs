using System.Collections.Generic;
using System.Threading.Tasks;
using Teashop.Backend.Domain.Order.Entities;

namespace Teashop.Backend.Application.Order.Repositories
{
    public interface IShippingMethodRepository
    {
        Task<IEnumerable<ShippingMethod>> GetAll();
        Task<bool> ExistsByName(string name);
    }
}
