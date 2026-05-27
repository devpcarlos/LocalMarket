using LocalMarket.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalMarket.Infrastructure.Persistence.Configuration
{
    public class ReviewConfig : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
                builder.ToTable("reviews", t =>
                t.HasCheckConstraint("CK_reviews_rating", "\"Rating\" >= 1 AND \"Rating\" <= 5"));
                builder.HasKey(x => x.Id);
                builder.HasIndex(x => new { x.BusinessId, x.UserId }).IsUnique();
                builder.Property(x => x.Rating).IsRequired();
                builder.Property(x => x.CreatedAt).HasDefaultValueSql("NOW()");
                builder.HasOne<Business>().WithMany().HasForeignKey(x => x.BusinessId).OnDelete(DeleteBehavior.Cascade);
                builder.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
