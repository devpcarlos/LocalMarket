
using LocalMarket.Core.Entities;

namespace LocalMarket.Core.Interfaces
{
   public interface IRefreshTokenRepository
    {
        Task SaveAsync(RefreshToken token);
        Task<RefreshToken?> GetByHashAsync(string tokenHash);
        Task RevokeAsync(RefreshToken token);
        Task RevokeAllByUserAsync(Guid userId);
    }
}
