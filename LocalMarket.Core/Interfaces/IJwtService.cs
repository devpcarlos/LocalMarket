using LocalMarket.Core.Entities;

namespace LocalMarket.Core.Interfaces
{
    public interface IJwtService
    {
        (string token, string jwtId) GenerateToken(User user);
        (string rawToken, string tokenHash) GenerateRefreshToken();
        string? ValidateTokenAndGetJwtId(string token);
        Guid? ValidateTokenAndGetUserId(string token);
    }
}
