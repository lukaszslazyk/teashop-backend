using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teashop.Backend.Domain.Cart.Entities;

namespace Teashop.Backend.Infrastructure.Persistence.Components.Cart.Configurations
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasKey(i => i.ItemId);
            builder.HasOne(i => i.Product)
                .WithMany();
        }
    }
}
