using Teashop.Backend.UI.Api.Product.Models;
using Entities = Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.UI.Api.Product.Mappings
{
    public static class ProductMappingExtensions
    {
        public static PresentationalProduct From(this PresentationalProduct presentationalProduct, Entities.Product product)
        {
            return new PresentationalProduct()
            {
                Id = product.Id,
                Name = product.Name,
                ReferencePrice = product.ReferencePrice,
                ReferenceGrams = product.ReferenceGrams
            };
        }

        public static Entities.Product ToProduct(this PresentationalProduct presentationalProduct)
        {
            return new Entities.Product()
            {
                Id = presentationalProduct.Id,
                Name = presentationalProduct.Name,
                ReferencePrice = presentationalProduct.ReferencePrice,
                ReferenceGrams = presentationalProduct.ReferenceGrams
            };
        }
    }
}
