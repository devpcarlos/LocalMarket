
using LocalMarket.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalMarket.Infrastructure.Persistence.Configuration
{
    public class BusinessConfig : IEntityTypeConfiguration<Business>
    {
        public void Configure(EntityTypeBuilder<Business> builder)
        {
                builder.ToTable("businesses");
                builder.HasKey(x => x.Id);
                builder.Property(x => x.Name).HasMaxLength(150).IsRequired();
                builder.Property(x => x.Description).HasMaxLength(1000);
                builder.Property(x => x.Nit).HasMaxLength(20);
                builder.Property(x => x.Address).HasMaxLength(200);
                builder.Property(x => x.City).HasMaxLength(80);
                builder.Property(x => x.Phone).HasMaxLength(20);
                builder.Property(x => x.IsActive).HasDefaultValue(true);
                builder.Property(x => x.IsVerified).HasDefaultValue(false);
                builder.Property(x => x.HasWhatsapp).HasDefaultValue(true);
                builder.Property(x => x.CreatedAt).HasDefaultValueSql("NOW()");
                builder.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
                builder.HasOne<BusinessCategory>().WithMany().HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
