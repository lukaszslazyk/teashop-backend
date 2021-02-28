using Microsoft.Extensions.DependencyInjection;
using Teashop.Backend.UI.Api.Cart.Mappings;
using Teashop.Backend.UI.Api.Cart.Session;
using Teashop.Backend.UI.Api.Commons.Filters.ApiExceptionFilter;
using Teashop.Backend.UI.Api.Commons.Session;
using Teashop.Backend.UI.Api.Order.Mappings;
using Teashop.Backend.UI.Api.Product.Mappings;

namespace Teashop.Backend.UI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUI(this IServiceCollection services)
        {
            services.AddTransient<ProductMapper>();
            services.AddTransient<CartMapper>();
            services.AddTransient<OrderMapper>();
            services.AddTransient<OrderMetaMapper>();
            services.AddTransient<SessionHandler>();
            services.AddTransient<SessionCartHandler>();
            services.AddControllers(options =>
                options.Filters.Add(new ApiExceptionFilterAttribute()));

            return services;
        }
    }
}
