using LocalMarket.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalMarket.Infrastructure.Persistence.Configuration
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
                builder.ToTable("products");
                builder.HasKey(x => x.Id);
                builder.Property(x => x.Name).HasMaxLength(150).IsRequired();
                builder.Property(x => x.Price).HasPrecision(12, 2);
                builder.Property(x => x.SalePrice).HasPrecision(12, 2);
                builder.Property(x => x.PhotoUrls).HasColumnType("text[]").HasConversion(
                    v => v.ToArray(),
                    v => v.ToList());
                builder.Property(x => x.IsAvailable).HasDefaultValue(true);
                builder.Property(x => x.CreatedAt).HasDefaultValueSql("NOW()");
                builder.HasOne<Business>().WithMany().HasForeignKey(x => x.BusinessId).OnDelete(DeleteBehavior.Cascade);
                builder.HasOne<ProductCategory>().WithMany().HasForeignKey(x => x.ProductCategoryId).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
