using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teashop.Backend.Domain.Product.Entities;

namespace Teashop.Backend.Infrastructure.Persistence.Components.Product.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.CategoryId);
            builder.HasAlternateKey(c => c.Name);
            builder.HasOne(c => c.ParentCategory)
                .WithMany();
        }
    }
}
