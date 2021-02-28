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
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.ShippingAddress)
                .WithOne()
                .HasForeignKey<OrderEntity>(o => o.ShippingAddressId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.BillingAddress)
                .WithOne()
                .HasForeignKey<OrderEntity>(o => o.BillingAddressId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.ChosenShippingMethod)
                .WithMany()
                .HasForeignKey(o => o.ChosenShippingMethodName)
                .IsRequired();

            builder.HasOne(o => o.ChosenPaymentMethod)
                .WithMany()
                .HasForeignKey(o => o.ChosenPaymentMethodName)
                .IsRequired();

            builder.HasOne(o => o.PaymentCard)
                .WithOne()
                .HasForeignKey<OrderEntity>(o => o.PaymentCardId);

            builder.Property(o => o.OrderNumber)
                .HasDefaultValueSql("NEXT VALUE FOR dbo.OrderNumbers");

            builder.HasIndex(o => o.OrderNumber)
                .IsUnique();

            builder.Property(o => o.CreatedAt)
                .HasDefaultValueSql("getdate()");

            builder.HasMany(o => o.OrderLines)
                .WithOne(line => line.Order);
        }
    }
}
