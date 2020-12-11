using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teashop.Backend.Domain.Order.Entities;

namespace Teashop.Backend.Infrastructure.Persistence.Components.Order.Configurations
{
    public class OrderEntityConfiguration : IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder.HasKey(o => o.OrderId);

            builder.HasOne(o => o.ContactInfo)
                .WithOne()
                .HasForeignKey<OrderEntity>(o => o.ContactInfoId)
                .IsRequired();

            builder.HasOne(o => o.ShippingAddress)
                .WithOne()
                .HasForeignKey<OrderEntity>(o => o.ShippingAddressId)
                .IsRequired();

            builder.HasOne(o => o.ChosenShippingMethod)
                .WithMany()
                .IsRequired();

            builder.HasOne(o => o.ChosenPaymentMethod)
                .WithMany()
                .IsRequired();

            builder.HasOne(o => o.PaymentCard)
                .WithOne()
                .HasForeignKey<OrderEntity>(o => o.PaymentCardId);

            builder.Property(o => o.OrderNo)
                .HasDefaultValueSql("NEXT VALUE FOR dbo.OrderNumbers");

            builder.HasIndex(o => o.OrderNo)
                .IsUnique();

            builder.Property(o => o.CreatedAt)
                .HasDefaultValueSql("getdate()");
        }
    }
}
