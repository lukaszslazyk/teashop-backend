using MediatR;
using System;
using Teashop.Backend.Domain.Order.Entities;

namespace Teashop.Backend.Application.Order.Queries.GetOrderById
{
    public class GetOrderByIdQuery : IRequest<OrderEntity>
    {
        public Guid OrderId { get; set; }
    }
}
