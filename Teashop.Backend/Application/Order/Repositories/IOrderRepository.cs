using System.Threading.Tasks;
using Teashop.Backend.Domain.Order.Entities;

namespace Teashop.Backend.Application.Order.Repositories
{
    public interface IOrderRepository
    {
        Task<OrderEntity> GetByOrderNo(int orderNo);
        Task Create(OrderEntity order);
    }
}
