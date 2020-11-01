using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teashop.Backend.Domain.Cart.Entities;

namespace Teashop.Backend.Infrastructure.Persistence.Components.Cart.Configurations
{
    public class CartEntityConfiguration : IEntityTypeConfiguration<CartEntity>
    {
        public void Configure(EntityTypeBuilder<CartEntity> builder)
        {
            builder.HasKey(c => c.CartId);
            builder.HasMany(c => c.Items)
                .WithOne(i => i.Cart);
        }
    }
}
