using LocalMarket.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

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

        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
