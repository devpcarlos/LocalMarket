using LocalMarket.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LocalMarket.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users => Set<User>();
        public DbSet<Business> Businesses => Set<Business>();
        public DbSet<BusinessCategory> BusinessCategories => Set<BusinessCategory>();
        public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Schedule> Schedules => Set<Schedule>();
        public DbSet<Conversation> Conversations => Set<Conversation>();
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<Favorite> Favorites => Set<Favorite>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<Subscription> Subscriptions => Set<Subscription>(); 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User
            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("users");
                e.HasKey(x => x.Id);
                e.Property(x => x.Name).HasMaxLength(100).IsRequired();
                e.Property(x => x.Email).HasMaxLength(150).IsRequired();
                e.HasIndex(x => x.Email).IsUnique();
                e.Property(x => x.Phone).HasMaxLength(20);
                e.Property(x => x.Role).HasMaxLength(20).HasDefaultValue("client");
                e.Property(x => x.PasswordHash).IsRequired();
                e.Property(x => x.CreatedAt).HasDefaultValueSql("NOW()");
            });

            // BusinessCategory
            modelBuilder.Entity<BusinessCategory>(e =>
            {
                e.ToTable("business_categories");
                e.HasKey(x => x.Id);
                e.Property(x => x.Name).HasMaxLength(80).IsRequired();
                e.Property(x => x.Icon).HasMaxLength(50);
                e.Property(x => x.CreatedAt).HasDefaultValueSql("NOW()");
            });

            // Business
            modelBuilder.Entity<Business>(e =>
            {
                e.ToTable("businesses");
                e.HasKey(x => x.Id);
                e.Property(x => x.Name).HasMaxLength(150).IsRequired();
                e.Property(x => x.Nit).HasMaxLength(20);
                e.Property(x => x.City).HasMaxLength(80);
                e.Property(x => x.Phone).HasMaxLength(20);
                e.Property(x => x.IsActive).HasDefaultValue(true);
                e.Property(x => x.IsVerified).HasDefaultValue(false);
                e.Property(x => x.HasWhatsapp).HasDefaultValue(true);
                e.Property(x => x.CreatedAt).HasDefaultValueSql("NOW()");
                e.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne<BusinessCategory>().WithMany().HasForeignKey(x => x.CategoryId);
            });

            // ProductCategory
            modelBuilder.Entity<ProductCategory>(e =>
            {
                e.ToTable("product_categories");
                e.HasKey(x => x.Id);
                e.Property(x => x.Name).HasMaxLength(100).IsRequired();
                e.Property(x => x.SortOrder).HasDefaultValue(0);
                e.Property(x => x.CreatedAt).HasDefaultValueSql("NOW()");
                e.HasOne<Business>().WithMany().HasForeignKey(x => x.BusinessId).OnDelete(DeleteBehavior.Cascade);
            });

            // Product
            modelBuilder.Entity<Product>(e =>
            {
                e.ToTable("products");
                e.HasKey(x => x.Id);
                e.Property(x => x.Name).HasMaxLength(150).IsRequired();
                e.Property(x => x.Price).HasPrecision(12, 2);
                e.Property(x => x.SalePrice).HasPrecision(12, 2);
                e.Property(x => x.PhotoUrls).HasColumnType("text[]");
                e.Property(x => x.IsAvailable).HasDefaultValue(true);
                e.Property(x => x.CreatedAt).HasDefaultValueSql("NOW()");
                e.HasOne<Business>().WithMany().HasForeignKey(x => x.BusinessId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne<ProductCategory>().WithMany().HasForeignKey(x => x.ProductCategoryId).OnDelete(DeleteBehavior.SetNull);
            });

            // Schedule
            modelBuilder.Entity<Schedule>(e =>
            {
                e.ToTable("schedules");
                e.HasKey(x => x.Id);
                e.HasIndex(x => new { x.BusinessId, x.DayOfWeek }).IsUnique();
                e.Property(x => x.IsClosed).HasDefaultValue(false);
                e.HasOne<Business>().WithMany().HasForeignKey(x => x.BusinessId).OnDelete(DeleteBehavior.Cascade);
            });

            // Conversation
            modelBuilder.Entity<Conversation>(e =>
            {
                e.ToTable("conversations");
                e.HasKey(x => x.Id);
                e.HasIndex(x => new { x.BusinessId, x.ClientId }).IsUnique();
                e.Property(x => x.CreatedAt).HasDefaultValueSql("NOW()");
                e.HasOne<Business>().WithMany().HasForeignKey(x => x.BusinessId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne<User>().WithMany().HasForeignKey(x => x.ClientId).OnDelete(DeleteBehavior.Cascade);
            });

            // Message
            modelBuilder.Entity<Message>(e =>
            {
                e.ToTable("messages");
                e.HasKey(x => x.Id);
                e.Property(x => x.Content).IsRequired();
                e.Property(x => x.Type).HasMaxLength(20).HasDefaultValue("text");
                e.Property(x => x.IsRead).HasDefaultValue(false);
                e.Property(x => x.SentAt).HasDefaultValueSql("NOW()");
                e.HasOne<Conversation>().WithMany().HasForeignKey(x => x.ConversationId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne<User>().WithMany().HasForeignKey(x => x.SenderId).OnDelete(DeleteBehavior.Cascade);
            });

            // Favorite
            modelBuilder.Entity<Favorite>(e =>
            {
                e.ToTable("favorites");
                e.HasKey(x => x.Id);
                e.HasIndex(x => new { x.UserId, x.BusinessId }).IsUnique();
                e.Property(x => x.CreatedAt).HasDefaultValueSql("NOW()");
                e.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne<Business>().WithMany().HasForeignKey(x => x.BusinessId).OnDelete(DeleteBehavior.Cascade);
            });

            // Review
            modelBuilder.Entity<Review>(e =>
            {
                e.ToTable("reviews");
                e.HasKey(x => x.Id);
                e.HasIndex(x => new { x.BusinessId, x.UserId }).IsUnique();
                e.Property(x => x.Rating).IsRequired();
                e.Property(x => x.CreatedAt).HasDefaultValueSql("NOW()");
                e.HasOne<Business>().WithMany().HasForeignKey(x => x.BusinessId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            });

            // Subscription
            modelBuilder.Entity<Subscription>(e =>
            {
                e.ToTable("subscriptions");
                e.HasKey(x => x.Id);
                e.Property(x => x.Plan).HasMaxLength(20).IsRequired();
                e.Property(x => x.MonthlyPrice).HasPrecision(12, 2);
                e.Property(x => x.IsActive).HasDefaultValue(false);
                e.Property(x => x.CreatedAt).HasDefaultValueSql("NOW()");
                e.HasOne<Business>().WithMany().HasForeignKey(x => x.BusinessId).OnDelete(DeleteBehavior.Cascade);
            });

            // Seed business categories
            modelBuilder.Entity<BusinessCategory>().HasData(
                new BusinessCategory { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Name = "Comida rápida", Icon = "pizza", CreatedAt = DateTime.UtcNow },
                new BusinessCategory { Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), Name = "Almuerzos", Icon = "pot", CreatedAt = DateTime.UtcNow },
                new BusinessCategory { Id = Guid.Parse("00000000-0000-0000-0000-000000000003"), Name = "Ropa y accesorios", Icon = "hanger", CreatedAt = DateTime.UtcNow },
                new BusinessCategory { Id = Guid.Parse("00000000-0000-0000-0000-000000000004"), Name = "Todo a 5 mil", Icon = "tag", CreatedAt = DateTime.UtcNow },
                new BusinessCategory { Id = Guid.Parse("00000000-0000-0000-0000-000000000005"), Name = "Farmacia", Icon = "pill", CreatedAt = DateTime.UtcNow },
                new BusinessCategory { Id = Guid.Parse("00000000-0000-0000-0000-000000000006"), Name = "Licores y cervezas", Icon = "beer", CreatedAt = DateTime.UtcNow },
                new BusinessCategory { Id = Guid.Parse("00000000-0000-0000-0000-000000000007"), Name = "Miscelánea", Icon = "store", CreatedAt = DateTime.UtcNow },
                new BusinessCategory { Id = Guid.Parse("00000000-0000-0000-0000-000000000008"), Name = "Panadería", Icon = "bread", CreatedAt = DateTime.UtcNow },
                new BusinessCategory { Id = Guid.Parse("00000000-0000-0000-0000-000000000009"), Name = "Frutas y verduras", Icon = "apple", CreatedAt = DateTime.UtcNow },
                new BusinessCategory { Id = Guid.Parse("00000000-0000-0000-0000-000000000010"), Name = "Papelería", Icon = "notebook", CreatedAt = DateTime.UtcNow }
            );
        }

    }
}
