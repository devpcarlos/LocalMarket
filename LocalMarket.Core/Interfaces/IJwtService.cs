using LocalMarket.Core.Entities;

namespace LocalMarket.Core.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        string GenerateRefreshToken();
    }
}
