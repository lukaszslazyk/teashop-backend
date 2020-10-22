using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Teashop.Backend.Configuration
{
    public static class TeashopCorsSetup
    {
        private const string AllowFrontendCorsPolicy = "AllowTeaShopFrontend";

        public static IServiceCollection AddTeaShopCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(AllowFrontendCorsPolicy, builder =>
                    builder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .WithOrigins(
                            configuration["Cors:AllowedHosts:Local"]
                        )
                );
            });

            return services;
        }
    }
}
