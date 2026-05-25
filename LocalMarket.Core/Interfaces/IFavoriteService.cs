
using LocalMarket.Core.DTos.Favorites;

namespace LocalMarket.Core.Interfaces
{
    public interface IFavoriteService
    {
        Task<List<FavoriteDto>> GetByUserIdAsync(Guid userId);
        Task<FavoriteDto> AddAsync(Guid userId, FavoriteDto dto);
        Task RemoveAsync(Guid userId, Guid businessId);
    }
}
