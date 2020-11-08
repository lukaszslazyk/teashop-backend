using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Infrastructure.Persistence.Components.Product.Configurations
{
    public class ProductEntityConfiguration : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.HasKey(p => p.ProductId);
        }
    }
}
