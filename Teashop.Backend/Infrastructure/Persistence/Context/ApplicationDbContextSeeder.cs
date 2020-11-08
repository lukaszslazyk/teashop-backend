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
            if (context.Products.Any() || context.Categories.Any())
                return;

            var tea = new Category
            {
                Name = "Tea"
            };
            var greenTea = new Category
            {
                Name = "GreenTea",
                ParentCategory = tea
            };
            var blackTea = new Category
            {
                Name = "BlackTea",
                ParentCategory = tea
            };
            var redTea = new Category
            {
                Name = "RedTea",
                ParentCategory = tea
            };
            var whiteTea = new Category
            {
                Name = "WhiteTea",
                ParentCategory = tea
            };
            var herbs = new Category
            {
                Name = "Herbs",
            };
            var accessories = new Category
            {
                Name = "Accessories",
            };

            context.Categories.Add(tea);
            context.Categories.Add(greenTea);
            context.Categories.Add(blackTea);
            context.Categories.Add(redTea);
            context.Categories.Add(whiteTea);
            context.Categories.Add(herbs);
            context.Categories.Add(accessories);

            var products = new List<ProductEntity>
            {
                new ProductEntity
                {
                    Name = "Sencha",
                    Price = 19.99,
                    QuantityPerPrice = 100,
                    Categories =
                    {
                        new ProductCategory
                        {
                            Category = greenTea
                        }
                    }
                },
                new ProductEntity
                {
                    Name = "Bancha",
                    Price = 9.99,
                    QuantityPerPrice = 100,
                    Categories =
                    {
                        new ProductCategory
                        {
                            Category = greenTea
                        }
                    }
                },
                new ProductEntity
                {
                    Name = "Gyokuro",
                    Price = 59.99,
                    QuantityPerPrice = 100,
                    Categories =
                    {
                        new ProductCategory
                        {
                            Category = greenTea
                        }
                    }
                },
                new ProductEntity
                {
                    Name = "Darjeeling",
                    Price = 19.99,
                    QuantityPerPrice = 100,
                    Categories =
                    {
                        new ProductCategory
                        {
                            Category = blackTea
                        }
                    }
                },
                new ProductEntity
                {
                    Name = "Pu-erh",
                    Price = 79.99,
                    QuantityPerPrice = 100,
                    Categories =
                    {
                        new ProductCategory
                        {
                            Category = redTea
                        }
                    }
                },
            };

            context.Products.AddRange(products);
        }
    }
}
