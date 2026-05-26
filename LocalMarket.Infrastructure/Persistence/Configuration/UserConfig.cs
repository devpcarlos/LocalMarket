
using LocalMarket.Core.Entities;
using LocalMarket.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalMarket.Infrastructure.Persistence.Configuration
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(150).IsRequired();
            builder.HasIndex(x => x.Email).IsUnique();
            builder.Property(x => x.Phone).HasMaxLength(20);
            builder.Property(x => x.Role).HasMaxLength(20)
                .HasConversion<string>()
                .HasDefaultValue(UserRole.Client);
            builder.Property(x => x.PasswordHash).IsRequired();
            builder.Property(x => x.CreatedAt).HasDefaultValueSql("NOW()");
          
        }
    }
}
