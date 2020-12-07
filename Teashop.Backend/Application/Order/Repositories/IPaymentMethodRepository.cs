using System.Collections.Generic;
using System.Threading.Tasks;
using Teashop.Backend.Domain.Order.Entities;

namespace Teashop.Backend.Application.Order.Repositories
{
    public interface IPaymentMethodRepository
    {
        Task<IEnumerable<PaymentMethod>> GetAll();
        Task<bool> ExistsByName(string name);
    }
}
