using System.Collections.Generic;
using System.Threading.Tasks;
using Teashop.Backend.Domain.Order.Entities;

namespace Teashop.Backend.Application.Order.Repositories
{
    public interface ICountryRepository
    {
        Task<IEnumerable<Country>> GetAll();
        Task<bool> ExistsByCode(string code);
    }
}
