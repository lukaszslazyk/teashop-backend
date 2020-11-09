using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Infrastructure.Persistence.Context
{
    public static class ApplicationDbContextSeeder
    {
        public static async Task Seed(ApplicationDbContext context)
        {
            await SeedProducts(context);
            context.SaveChanges();
        }

        private static async Task SeedProducts(ApplicationDbContext context)
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

            await context.Categories.AddAsync(tea);
            await context.Categories.AddAsync(greenTea);
            await context.Categories.AddAsync(blackTea);
            await context.Categories.AddAsync(redTea);
            await context.Categories.AddAsync(whiteTea);
            await context.Categories.AddAsync(herbs);
            await context.Categories.AddAsync(accessories);

            var products = new List<ProductEntity>
            {
                new ProductEntity
                {
                    Name = "Sencha",
                    Price = 19.99,
                    QuantityPerPrice = 100,
                    ImagePath="images/leaves_example.jpg",
                    ProductCategories =
                    {
                        new ProductCategory
                        {
                            Category = tea
                        },
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
                    ImagePath="images/leaves_example.jpg",
                    ProductCategories =
                    {
                        new ProductCategory
                        {
                            Category = tea
                        },
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
                    ImagePath="images/leaves_example.jpg",
                    ProductCategories =
                    {
                        new ProductCategory
                        {
                            Category = tea
                        },
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
                    ImagePath="images/leaves_example.jpg",
                    ProductCategories =
                    {
                        new ProductCategory
                        {
                            Category = tea
                        },
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
                    ImagePath="images/leaves_example.jpg",
                    ProductCategories =
                    {
                        new ProductCategory
                        {
                            Category = tea
                        },
                        new ProductCategory
                        {
                            Category = redTea
                        }
                    }
                },
            };

            await context.Products.AddRangeAsync(products);
        }
    }
}
