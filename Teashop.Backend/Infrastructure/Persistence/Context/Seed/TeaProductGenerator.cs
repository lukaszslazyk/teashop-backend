using System;
using System.Collections.Generic;
using System.Linq;
using Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Infrastructure.Persistence.Context.Seed
{
    public class TeaProductGenerator
    {
        private readonly Dictionary<string, string[]> _namePart1Values = new Dictionary<string, string[]>
        {
            { "GreenTea", new string[]
                {
                    "Bancha",
                    "Sencha",
                    "Gyokuro",
                    "Shincha",
                    "Sejak",
                    "Long Jing",
                    "Gunpowder",
                }
            },
            { "BlackTea", new string[]
                {
                    "Darjeeling",
                    "Asam",
                    "Ceylon",
                    "Earl Gray",
                }
            },
            { "RedTea", new string[]
                {
                    "Pu-Erh",
                }
            },
            { "WhiteTea", new string[]
                {
                    "Pai Mu Tan",
                    "Silver Needle",
                    "White Peony",
                }
            },
        };

        private readonly string[] _namePart2Values = new string[]
        {
            "",
            "Pure",
            "Citrus",
            "Jasmine",
            "Hibiscus",
            "Rose",
            "Cherry",
            "Mint",
            "Aloe",
            "Forest Fruits",
            "Tropical Fruits",
        };

        private readonly string[] _namePart3Values = new string[]
        {
            "",
            "Classic",
            "Organic",
            "First Flush",
            "Special Selection",
            "Diet",
            "Energy",
            "Relax",
            "Fresh",
            "Aromatic",
            "Sweet"
        };

        private readonly (int, int) _descriptionWordsRange = (25, 150);
        private readonly (int, int) _priceRange = (5, 50);
        private readonly (int, int) _brewingTemperatureRange = (60, 95);
        private readonly int[] _brewingTimeValues = { 1, 2, 3, 5 };

        private readonly Category _teaMainCategory;
        private readonly Random _random;

        public TeaProductGenerator(Category teaMainCategory)
        {
            _teaMainCategory = teaMainCategory;
            _random = new Random();
        }

        public IList<ProductEntity> GenerateMultipleTeaProducts(Category category, int numberOfProducts)
        {
            return Enumerable.Range(0, numberOfProducts)
                .Select(x => GenerateTeaProduct(category))
                .ToList();
        }

        private ProductEntity GenerateTeaProduct(Category category)
        {
            return new ProductEntity
            {
                Name = GenerateTeaName(category),
                Description = GenerateDescription(),
                ImagePath = "images/leaves_example.jpg",
                Price = GeneratePrice(),
                QuantityPerPrice = 100,
                BrewingInfo = GenerateBrewingInfo(),
                ProductCategories =
                {
                    new ProductCategory
                    {
                        Category = _teaMainCategory
                    },
                    new ProductCategory
                    {
                        Category = category
                    }
                },
            };
        }

        private string GenerateTeaName(Category category)
        {
            var part1 = _namePart1Values[category.Name][_random.Next(_namePart1Values[category.Name].Length)];
            var part2 = _namePart2Values[_random.Next(_namePart2Values.Length)];
            var part3 = _namePart3Values[_random.Next(_namePart3Values.Length)];

            return $"{part1} {part2} {part3}"
                .Trim()
                .Replace("  ", " ");
        }

        private string GenerateDescription()
        {
            return SampleTextGenerator.Generate(_random.Next(_descriptionWordsRange.Item1, _descriptionWordsRange.Item2));
        }

        private double GeneratePrice()
        {
            return RoundToNearestN(_random.Next(_priceRange.Item1, _priceRange.Item2), 5.0) - 0.01;
        }

        private double RoundToNearestN(double input, double N)
        {
            return Math.Round(input / N) * N;
        }

        private BrewingInfo GenerateBrewingInfo()
        {
            return new BrewingInfo
            {
                WeightInfo = "3g / 250 ml",
                TemperatureInfo = $"{GetRandomBrewingTemperature()} degrees Celsius",
                TimeInfo = $"{GetRandomBrewingTime()} min",
                NumberOfBrewingsInfo = "2-3 brewings"
            };
        }

        private int GetRandomBrewingTemperature()
        {
            return (int)RoundToNearestN(_random.Next(_brewingTemperatureRange.Item1, _brewingTemperatureRange.Item2), 5.0);
        }

        private int GetRandomBrewingTime()
        {
            return _brewingTimeValues[_random.Next(_brewingTimeValues.Length)];
        }
    }
}
