using LocalMarket.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LocalMarket.Infrastructure.Persistence.Configuration
{
    public class BusinessCategoryConfig : IEntityTypeConfiguration<BusinessCategory>
    {
        public void Configure(EntityTypeBuilder<BusinessCategory> builder)
        {           
                builder .ToTable("business_categories");
                builder.HasKey(x => x.Id);
                builder.Property(x => x.Name).HasMaxLength(80).IsRequired();
                builder.Property(x => x.Icon).HasMaxLength(50);
                builder.Property(x => x.CreatedAt).HasDefaultValueSql("NOW()");

          builder.HasData(
     new BusinessCategory { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Name = "Comida rápida", Icon = "pizza", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
     new BusinessCategory { Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), Name = "Almuerzos", Icon = "pot", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
     new BusinessCategory { Id = Guid.Parse("00000000-0000-0000-0000-000000000003"), Name = "Ropa y accesorios", Icon = "hanger", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
     new BusinessCategory { Id = Guid.Parse("00000000-0000-0000-0000-000000000004"), Name = "Todo a 5 mil", Icon = "tag", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
     new BusinessCategory { Id = Guid.Parse("00000000-0000-0000-0000-000000000005"), Name = "Farmacia", Icon = "pill", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
     new BusinessCategory { Id = Guid.Parse("00000000-0000-0000-0000-000000000006"), Name = "Licores y cervezas", Icon = "beer", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
     new BusinessCategory { Id = Guid.Parse("00000000-0000-0000-0000-000000000007"), Name = "Miscelánea", Icon = "store", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
     new BusinessCategory { Id = Guid.Parse("00000000-0000-0000-0000-000000000008"), Name = "Panadería", Icon = "bread", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
     new BusinessCategory { Id = Guid.Parse("00000000-0000-0000-0000-000000000009"), Name = "Frutas y verduras", Icon = "apple", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
     new BusinessCategory { Id = Guid.Parse("00000000-0000-0000-0000-000000000010"), Name = "Papelería", Icon = "notebook", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
 );
        }
    }
}
