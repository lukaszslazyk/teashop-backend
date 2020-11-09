﻿using System.Collections.Generic;
using System.Linq;
using Teashop.Backend.Domain.Product.Entities;
using Teashop.Backend.UI.Api.Product.Models;

namespace Teashop.Backend.UI.Api.Product.Mappings
{
    public class ProductMapper
    {
        public PresentationalProduct MapToPresentational(ProductEntity product)
        {
            return new PresentationalProduct()
            {
                Id = product.ProductId,
                Name = product.Name,
                Price = product.Price,
                QuantityPerPrice = product.QuantityPerPrice,
                ImagePath = product.ImagePath,
                Categories = product.ProductCategories
                    .Select(pc => pc.Category.Name)
                    .ToArray()
            };
        }

        public IEnumerable<PresentationalProduct> MapToMultiplePresentationals(IEnumerable<ProductEntity> products)
        {
            return products
                .Select(product => MapToPresentational(product));
        }

        public ProductEntity MapFromPresentational(PresentationalProduct presentationalProduct)
        {
            return new ProductEntity()
            {
                ProductId = presentationalProduct.Id,
                Name = presentationalProduct.Name,
                Price = presentationalProduct.Price,
                QuantityPerPrice = presentationalProduct.QuantityPerPrice
            };
        }
    }
}