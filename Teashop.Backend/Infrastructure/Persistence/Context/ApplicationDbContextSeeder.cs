using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Teashop.Backend.Domain.Order.Entities;
using Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Infrastructure.Persistence.Context
{
    public static class ApplicationDbContextSeeder
    {
        public static async Task Seed(ApplicationDbContext context)
        {
            await SeedOrder(context);
            await SeedProduct(context);
            context.SaveChanges();
        }

        private static async Task SeedOrder(ApplicationDbContext context)
        {
            if (context.Countries.Any()
                || context.ShippingMethods.Any()
                || context.PaymentMethods.Any())
                return;

            var countries = new List<Country>
            {
                new Country
                {
                    Code = "US",
                    Name = "United States",
                },
                new Country
                {
                    Code = "UK",
                    Name = "United Kingdom",
                },
            };

            var shippingMethods = new List<ShippingMethod>
            {
                new ShippingMethod
                {
                    Name = "standard",
                    DisplayName = "Standard Delivery",
                    Price = 9.99,
                },
            };

            var paymentMethods = new List<PaymentMethod>
            {
                new PaymentMethod
                {
                    Name = "creditCard",
                    DisplayName = "Credit Card",
                },
            };

            await context.Countries.AddRangeAsync(countries);
            await context.ShippingMethods.AddRangeAsync(shippingMethods);
            await context.PaymentMethods.AddRangeAsync(paymentMethods);
        }

        private static async Task SeedProduct(ApplicationDbContext context)
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
                    Description = GetLoremIpsum(),
                    BrewingInfo = new BrewingInfo
                    {
                        WeightInfo = "3g / 250 ml",
                        TemperatureInfo = "80 degrees Celsius",
                        TimeInfo = "2 min",
                        NumberOfBrewingsInfo = "3 brewings"
                    },
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
                    Description = GetLoremIpsum(),
                    BrewingInfo = new BrewingInfo
                    {
                        WeightInfo = "3g / 250 ml",
                        TemperatureInfo = "85 degrees Celsius",
                        TimeInfo = "2 min",
                        NumberOfBrewingsInfo = "3 brewings"
                    },
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
                    Description = GetLoremIpsum(),
                    BrewingInfo = new BrewingInfo
                    {
                        WeightInfo = "3g / 250 ml",
                        TemperatureInfo = "60 degrees Celsius",
                        TimeInfo = "2 min",
                        NumberOfBrewingsInfo = "2-3 brewings"
                    },
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
                    Description = GetLoremIpsum(),
                    BrewingInfo = new BrewingInfo
                    {
                        WeightInfo = "3g / 250 ml",
                        TemperatureInfo = "96 degrees Celsius",
                        TimeInfo = "2-3 min",
                        NumberOfBrewingsInfo = "3 brewings"
                    },
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
                    Description = GetLoremIpsum(),
                    BrewingInfo = new BrewingInfo
                    {
                        WeightInfo = "5g / 250 ml",
                        TemperatureInfo = "96 degrees Celsius",
                        TimeInfo = "2-3 min"
                    },
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
                new ProductEntity
                {
                    Name = "Kettle",
                    Price = 29.99,
                    QuantityPerPrice = 1,
                    Description = GetLoremIpsum(),
                    ProductCategories =
                    {
                        new ProductCategory
                        {
                            Category = accessories
                        },
                    }
                },
            };

            await context.Products.AddRangeAsync(products);
        }

        private static string GetLoremIpsum()
        {
            return "Lorem ipsum dolor sit amet, consectetur adipiscing elit." +
                " Fusce ullamcorper congue tincidunt. Vivamus elementum mattis" +
                " quam, non ultrices erat hendrerit sit amet. Nullam quis metus" +
                " varius, aliquet nulla at, ullamcorper massa. Etiam fringilla" +
                " quam in odio egestas, nec malesuada risus mattis. Nunc suscipit," +
                " neque in tincidunt ullamcorper, diam velit pretium neque," +
                " aliquet malesuada tortor erat vel ante. Vestibulum non massa" +
                " in velit porta fermentum. Etiam in faucibus augue. Pellentesque" +
                " fermentum tortor id tincidunt rutrum. Class aptent taciti sociosqu" +
                " ad litora torquent per conubia nostra, per inceptos himenaeos." +
                " Integer dapibus nisi et neque ullamcorper vehicula. Aliquam tempor" +
                " sit amet tellus venenatis ultricies. Aliquam sed ante massa. " +
                "Morbi vel ipsum est. Quisque ut lacus condimentum, vestibulum leo et," +
                " aliquet mauris. ";
        }
    }
}
