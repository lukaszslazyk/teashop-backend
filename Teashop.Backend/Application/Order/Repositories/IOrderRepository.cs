using System;
using System.Threading.Tasks;
using Teashop.Backend.Domain.Order.Entities;

namespace Teashop.Backend.Application.Order.Repositories
{
    public interface IOrderRepository
    {
        Task<OrderEntity> GetById(Guid orderId);
        Task Create(OrderEntity order);
    }
}
