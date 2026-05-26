using LocalMarket.Core.Entities;
using LocalMarket.Core.Interfaces;
using LocalMarket.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LocalMarket.Infrastructure.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _dbContext;

        public ReviewRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Review>> GetByBusinessIdAsync(Guid businessId)
        {
            return await _dbContext.Reviews
                .Where(r => r.BusinessId == businessId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<Review?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Reviews.FindAsync(id);
        }

        public async Task<Review?> GetByUserAndBusinessAsync(Guid userId, Guid businessId)
        {
            return await _dbContext.Reviews
                .FirstOrDefaultAsync(r => r.UserId == userId && r.BusinessId == businessId);
        }

        public async Task<Review> CreateAsync(Review review)
        {
            await _dbContext.Reviews.AddAsync(review);
            await _dbContext.SaveChangesAsync();
            return review;
        }

        public async Task<Review> UpdateAsync(Review review)
        {
            _dbContext.Reviews.Update(review);
            await _dbContext.SaveChangesAsync();
            return review;
        }

        public async Task DeleteAsync(Review review)
        {
            _dbContext.Reviews.Remove(review);
            await _dbContext.SaveChangesAsync();
        }
    }
}
