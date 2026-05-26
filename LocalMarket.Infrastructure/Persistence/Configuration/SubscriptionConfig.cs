using LocalMarket.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalMarket.Infrastructure.Persistence.Configuration
{
    public class SubscriptionConfig : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.ToTable("subscriptions");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Plan).HasMaxLength(20).IsRequired();
            builder.Property(x => x.MonthlyPrice).HasPrecision(12, 2);
            builder.Property(x => x.IsActive).HasDefaultValue(false);
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("NOW()");
            builder.HasOne<Business>().WithMany().HasForeignKey(x => x.BusinessId).OnDelete(DeleteBehavior.Cascade);
        }        
    }
}
