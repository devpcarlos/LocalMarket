using LocalMarket.Core.Entities;
using LocalMarket.Core.Interfaces;
using LocalMarket.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LocalMarket.Infrastructure.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly AppDbContext _dbContext;

        public FavoriteRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Favorite>> GetByUserIdAsync(Guid userId)
        {
            return await _dbContext.Favorites
                .Where(f => f.UserId == userId)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
        }

        public async Task<Favorite?> GetByUserAndBusinessAsync(Guid userId, Guid businessId)
        {
            return await _dbContext.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.BusinessId == businessId);
        }

        public async Task<Favorite> CreateAsync(Favorite favorite)
        {
            await _dbContext.Favorites.AddAsync(favorite);
            await _dbContext.SaveChangesAsync();
            return favorite;
        }

        public async Task DeleteAsync(Favorite favorite)
        {
                _dbContext.Favorites.Remove(favorite);
                await _dbContext.SaveChangesAsync();            
        }
    }
}
