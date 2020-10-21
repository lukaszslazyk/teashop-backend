using Microsoft.Extensions.DependencyInjection;

namespace Teashop.Backend.UI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUI(this IServiceCollection services)
        {
            services.AddControllers();

            return services;
        }
    }
}
