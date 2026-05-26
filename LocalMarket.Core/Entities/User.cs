using LocalMarket.Core.Enums;

namespace LocalMarket.Core.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public UserRole Role { get; set; } = UserRole.Client;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
