using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Teashop.Backend.Infrastructure.Persistence.Context;
using Teashop.Backend.Infrastructure.Persistence.Context.Seed;

namespace Teashop.Backend.Configuration
{
    public static class IHostExtensions
    {
        public static async Task<IHost> MigrateAndSeedDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    context.Database.Migrate();
                    await ApplicationDbContextSeeder.Seed(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating or seeding the database.");
                    throw ex;
                }
            }

            return host;
        }
    }
}
