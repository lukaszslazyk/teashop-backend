using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Teashop.Backend.Application.Cart.Repositories;
using Teashop.Backend.Application.Order.Repositories;
using Teashop.Backend.Application.Product.Repositories;
using Teashop.Backend.Infrastructure.Persistence.Components.Cart.Repositories;
using Teashop.Backend.Infrastructure.Persistence.Components.Order.Repositories;
using Teashop.Backend.Infrastructure.Persistence.Components.Product.Repositories;
using Teashop.Backend.Infrastructure.Persistence.Context;

namespace Teashop.Backend.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(opt =>
                opt.UseSqlServer(configuration.GetConnectionString("Database"))
            );
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IShippingMethodRepository, ShippingMethodRepository>();
            services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            return services;
        }
    }
}
