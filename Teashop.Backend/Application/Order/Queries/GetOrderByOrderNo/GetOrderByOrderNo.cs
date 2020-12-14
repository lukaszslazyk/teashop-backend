using MediatR;
using Teashop.Backend.Domain.Order.Entities;

namespace Teashop.Backend.Application.Order.Queries.GetOrderById
{
    public class GetOrderByOrderNo : IRequest<OrderEntity>
    {
        public int OrderNo { get; set; }
    }
}
