using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teashop.Backend.Domain.Order.Entities;

namespace Teashop.Backend.Infrastructure.Persistence.Components.Order.Configurations
{
    public class PaymentCardConfiguration : IEntityTypeConfiguration<PaymentCard>
    {
        public void Configure(EntityTypeBuilder<PaymentCard> builder)
        {
            builder.HasKey(cc => cc.PaymentCardId);
        }
    }
}
