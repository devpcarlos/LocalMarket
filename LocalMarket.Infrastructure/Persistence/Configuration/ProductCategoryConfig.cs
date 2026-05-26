using LocalMarket.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalMarket.Infrastructure.Persistence.Configuration
{
    public class ProductCategoryConfig : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
                builder.ToTable("product_categories");
                builder.HasKey(x => x.Id);
                builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
                builder.Property(x => x.SortOrder).HasDefaultValue(0);
                builder.Property(x => x.CreatedAt).HasDefaultValueSql("NOW()");
                builder.HasOne<Business>().WithMany().HasForeignKey(x => x.BusinessId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
