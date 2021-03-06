using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Teashop.Backend.Application.Order.Repositories;
using Teashop.Backend.Domain.Order.Entities;
using Teashop.Backend.Infrastructure.Persistence.Context;

namespace Teashop.Backend.Infrastructure.Persistence.Components.Order.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OrderEntity> GetById(Guid orderId)
        {
            return await _context
                .Orders
                .Include(o => o.ContactInfo)
                .Include(o => o.ShippingAddress)
                    .ThenInclude(sa => sa.Country)
                .Include(o => o.BillingAddress)
                    .ThenInclude(ba => ba.Country)
                .Include(o => o.ChosenShippingMethod)
                .Include(o => o.ChosenPaymentMethod)
                .Include(o => o.PaymentCard)
                .Include(o => o.OrderLines)
                    .ThenInclude(line => line.Product)
                .Where(o => o.OrderId == orderId)
                .FirstOrDefaultAsync();
        }

        public async Task Create(OrderEntity order)
        {
            _context.ContactInfos.Add(order.ContactInfo);
            _context.Addresses.Add(order.ShippingAddress);
            _context.Addresses.Add(order.BillingAddress);
            if (order.PaymentCard != null)
                _context.PaymentCards.Add(order.PaymentCard);
            _context.Orders.Add(order);
            _context.OrderLines.AddRange(order.OrderLines);
            await _context.SaveChangesAsync();
        }
    }
}
