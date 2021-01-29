using System.Threading.Tasks;

namespace Teashop.Backend.Infrastructure.Persistence.Context.Seed
{
    public static class ApplicationDbContextSeeder
    {
        public static async Task Seed(ApplicationDbContext context)
        {
            await OrderSeeder.Seed(context);
            await ProductSeeder.Seed(context);
            context.SaveChanges();
        }
    }
}
