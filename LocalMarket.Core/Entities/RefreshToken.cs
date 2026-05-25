
namespace LocalMarket.Core.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string TokenHash { get; set; } = string.Empty;
        public string JwtId { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? IpAddress { get; set; }
        public bool IsRevoked { get; set; } = false;

        public bool IsActive => !IsRevoked && DateTime.UtcNow < ExpiresAt;
    }
}
