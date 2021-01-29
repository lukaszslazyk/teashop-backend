using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Teashop.Backend.Domain.Order.Entities;

namespace Teashop.Backend.Infrastructure.Persistence.Context.Seed
{
    public static class OrderSeeder
    {
        public static async Task Seed(ApplicationDbContext context)
        {
            if (context.Countries.Any()
                || context.ShippingMethods.Any()
                || context.PaymentMethods.Any())
                return;

            var countries = new List<Country>
            {
                new Country
                {
                    Code = "US",
                    Name = "United States",
                },
                new Country
                {
                    Code = "UK",
                    Name = "United Kingdom",
                },
            };

            var shippingMethods = new List<ShippingMethod>
            {
                new ShippingMethod
                {
                    Name = "standard",
                    DisplayName = "Standard delivery",
                    Fee = 9.99,
                    ShippingMethodNo = 1,
                },
                new ShippingMethod
                {
                    Name = "fast",
                    DisplayName = "Fast delivery",
                    Fee = 15.99,
                    ShippingMethodNo = 2,
                },
            };

            var paymentMethods = new List<PaymentMethod>
            {
                new PaymentMethod
                {
                    Name = "card",
                    DisplayName = "Credit/Debit Card",
                    Fee = 0.0,
                    PaymentMethodNo = 1,
                },
                new PaymentMethod
                {
                    Name = "cashOnDelivery",
                    DisplayName = "Cash on delivery",
                    Fee = 4.99,
                    PaymentMethodNo = 2,
                },
            };

            await context.Countries.AddRangeAsync(countries);
            await context.ShippingMethods.AddRangeAsync(shippingMethods);
            await context.PaymentMethods.AddRangeAsync(paymentMethods);
        }
    }
}
