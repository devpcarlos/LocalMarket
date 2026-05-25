using LocalMarket.Core.Entities;

namespace LocalMarket.Core.Interfaces
{
    public interface IFavoriteRepository
    {
        Task<List<Favorite>> GetByUserIdAsync(Guid userId);
        Task<Favorite?> GetByUserAndBusinessAsync(Guid userId, Guid businessId);
        Task<Favorite> CreateAsync(Favorite favorite);
        Task DeleteAsync(Guid id);
    }
}
