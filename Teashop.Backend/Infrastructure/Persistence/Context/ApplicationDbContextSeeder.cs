using System.Collections.Generic;
using System.Linq;
using Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Infrastructure.Persistence.Context
{
    public static class ApplicationDbContextSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            SeedProducts(context);
            context.SaveChanges();
        }

        private static void SeedProducts(ApplicationDbContext context)
        {
            if (context.Products.Any())
                return;

            var products = new List<Product>
            {
                new Product
                {
                    Id = "1",
                    Name = "Sencha",
                    ReferencePrice = 19.99F,
                    ReferenceGrams = 100
                },
            };

            context.Products.AddRange(products);
        }
    }
}
