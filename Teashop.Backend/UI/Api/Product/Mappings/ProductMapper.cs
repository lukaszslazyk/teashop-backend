using System.Collections.Generic;
using System.Linq;
using Teashop.Backend.Application.Commons.Models;
using Teashop.Backend.Domain.Product.Entities;
using Teashop.Backend.UI.Api.Product.Models;

namespace Teashop.Backend.UI.Api.Product.Mappings
{
    public class ProductMapper
    {
        public PresentationalProduct MapToPresentational(ProductEntity product)
        {
            return new PresentationalProduct
            {
                Id = product.ProductId,
                ProductNumber = product.ProductNumber,
                Name = product.Name,
                Price = product.Price,
                QuantityPerPrice = product.QuantityPerPrice,
                ImagePath = product.ImagePath,
                Description = product.Description,
                BrewingInfo = MapToPresentational(product.BrewingInfo),
                Categories = product.ProductCategories
                    .Select(pc => pc.Category.Name)
                    .ToArray()
            };
        }

        public ProductEntity MapFromPresentational(PresentationalProduct presentationalProduct)
        {
            return new ProductEntity
            {
                ProductId = presentationalProduct.Id,
                Name = presentationalProduct.Name,
                Price = presentationalProduct.Price,
                QuantityPerPrice = presentationalProduct.QuantityPerPrice
            };
        }

        public PresentationalBrewingInfo MapToPresentational(BrewingInfo brewingInfo)
        {
            if (brewingInfo == null)
                return null;

            return new PresentationalBrewingInfo
            {
                WeightInfo = brewingInfo.WeightInfo,
                TemperatureInfo = brewingInfo.TemperatureInfo,
                TimeInfo = brewingInfo.TimeInfo,
                NumberOfBrewingsInfo = brewingInfo.NumberOfBrewingsInfo
            };
        }

        public GetProductsBySpecificationPaginatedResponse MapToGetProductsBySpecificationResponse(PaginatedList<ProductEntity> result)
        {
            return new GetProductsBySpecificationPaginatedResponse
            {
                PageIndex = result.PageIndex,
                PageSize = result.PageSize,
                PagesInTotal = result.PagesInTotal,
                TotalCount = result.TotalCount,
                Products = MapToMultipleMinimized(result.Items)
            };
        }

        public List<MinimizedPresentationalProduct> MapToMultipleMinimized(List<ProductEntity> products)
        {
            return products
                .Select(product => MapToMinimized(product))
                .ToList();
        }

        public MinimizedPresentationalProduct MapToMinimized(ProductEntity product)
        {
            return new MinimizedPresentationalProduct
            {
                Id = product.ProductId,
                ProductNumber = product.ProductNumber,
                Name = product.Name,
                Price = product.Price,
                QuantityPerPrice = product.QuantityPerPrice,
                ImagePath = product.ImagePath,
            };
        }
    }
}
