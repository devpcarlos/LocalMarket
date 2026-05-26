using LocalMarket.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace LocalMarket.Infrastructure.Persistence.Configuration
{
    public class RefreshTokenConfig : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("refresh_tokens");
                builder.HasKey(x => x.Id);
                builder.Property(x => x.TokenHash).HasMaxLength(128).IsRequired();
                builder.HasIndex(x => x.TokenHash).IsUnique();
                builder.Property(x => x.JwtId).HasMaxLength(36).IsRequired();
                builder.Property(x => x.IpAddress).HasMaxLength(45);
                builder.Property(x => x.IsRevoked).HasDefaultValue(false);
                builder.Property(x => x.CreatedAt).HasDefaultValueSql("NOW()");
                builder.Ignore(x => x.IsActive);
                builder.HasOne<User>().WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
