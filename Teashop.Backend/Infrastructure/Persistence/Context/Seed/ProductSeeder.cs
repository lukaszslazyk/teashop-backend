using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Infrastructure.Persistence.Context.Seed
{
    public class ProductSeeder
    {
        public static async Task Seed(ApplicationDbContext context)
        {
            if (context.Products.Any() || context.Categories.Any())
                return;

            var teaCategory = new Category
            {
                Name = "Tea"
            };
            var greenTeaCategory = new Category
            {
                Name = "GreenTea",
                ParentCategory = teaCategory
            };
            var blackTeaCategory = new Category
            {
                Name = "BlackTea",
                ParentCategory = teaCategory
            };
            var redTeaCategory = new Category
            {
                Name = "RedTea",
                ParentCategory = teaCategory
            };
            var whiteTeaCategory = new Category
            {
                Name = "WhiteTea",
                ParentCategory = teaCategory
            };
            var herbsCategory = new Category
            {
                Name = "Herbs",
            };
            var accessoriesCategory = new Category
            {
                Name = "Accessories",
            };
            var recommendedCategory = new Category
            {
                Name = "Recommended",
            };
            await context.Categories.AddAsync(teaCategory);
            await context.Categories.AddAsync(greenTeaCategory);
            await context.Categories.AddAsync(blackTeaCategory);
            await context.Categories.AddAsync(redTeaCategory);
            await context.Categories.AddAsync(whiteTeaCategory);
            await context.Categories.AddAsync(herbsCategory);
            await context.Categories.AddAsync(accessoriesCategory);

            var products = new List<ProductEntity>();
            var teaProductGenerator = new TeaProductGenerator(teaCategory);
            products.AddRange(teaProductGenerator.GenerateMultipleTeaProducts(greenTeaCategory, 30));
            products.AddRange(teaProductGenerator.GenerateMultipleTeaProducts(blackTeaCategory, 15));
            products.AddRange(teaProductGenerator.GenerateMultipleTeaProducts(redTeaCategory, 10));
            products.AddRange(teaProductGenerator.GenerateMultipleTeaProducts(whiteTeaCategory, 10));
            products.AddRange(GetAccesssories(accessoriesCategory));
            AssignRandomProductsToCategory(products, recommendedCategory, 5);
            await context.Products.AddRangeAsync(products);
        }

        private static List<ProductEntity> GetAccesssories(Category accessoriesCategory)
        {
            return new List<ProductEntity>
            {
                new ProductEntity
                {
                    Name = "Kettle",
                    Price = 29.99,
                    QuantityPerPrice = 1,
                    Description = SampleTextGenerator.Generate(100),
                    ProductCategories =
                    {
                        new ProductCategory
                        {
                            Category = accessoriesCategory
                        },
                    }
                },
            };
        }

        private static void AssignRandomProductsToCategory(List<ProductEntity> products, Category category, int numberOfProducts)
        {
            var random = new Random();
            products
                .OrderBy(x => random.Next())
                .Take(numberOfProducts)
                .ToList()
                .ForEach(p => p.ProductCategories.Add(new ProductCategory() { Category = category }));
        }
    }
}