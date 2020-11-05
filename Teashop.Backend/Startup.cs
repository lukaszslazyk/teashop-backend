using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Teashop.Backend.Application;
using Teashop.Backend.Configuration;
using Teashop.Backend.Infrastructure;
using Teashop.Backend.Infrastructure.Persistence.Context;
using Teashop.Backend.UI;

namespace Teashop.Backend
{
    public class Startup
    {
        private const string AllowFrontendCorsPolicy = "AllowTeaShopFrontend";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSession(opt =>
            {
                opt.Cookie.Name = ".Teashop.Session";
                opt.Cookie.HttpOnly = true;
                opt.Cookie.IsEssential = true;
            });

            services.AddTeaShopCors(Configuration);
            services.AddApplication();
            services.AddInfrastructure(Configuration);
            services.AddUI();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler("/error");
            app.UseRouting();
            app.UseCors(AllowFrontendCorsPolicy);
            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            ApplyMigrationsWithSeed(app);
        }

        private static void ApplyMigrationsWithSeed(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                context.Database.Migrate();
                ApplicationDbContextSeeder.Seed(context);
            }
        }
    }
}
