using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Teashop.Backend.Application.Cart.Repositories;
using Teashop.Backend.Application.Product.Repositories;
using Teashop.Backend.Infrastructure.Persistence.Components.Cart.Repositories;
using Teashop.Backend.Infrastructure.Persistence.Components.Product.Repositories;
using Teashop.Backend.Infrastructure.Persistence.Context;

namespace Teashop.Backend.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddDbContext<ApplicationDbContext>(opt =>
                opt.UseSqlServer(configuration.GetConnectionString("Database"))
            );

            return services;
        }
    }
}
