using System;
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

            var products = new List<ProductEntity>
            {
                new ProductEntity
                {
                    Name = "Sencha",
                    Price = 19.99,
                    QuantityPerPrice = 100
                },
                new ProductEntity
                {
                    Name = "Bancha",
                    Price = 9.99,
                    QuantityPerPrice = 100
                },
                new ProductEntity
                {
                    Name = "Gyokuro",
                    Price = 59.99,
                    QuantityPerPrice = 100
                },
            };

            context.Products.AddRange(products);
        }
    }
}
