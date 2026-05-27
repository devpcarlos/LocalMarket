

using LocalMarket.Core.Entities;
using LocalMarket.Core.Interfaces;
using LocalMarket.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LocalMarket.Infrastructure.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly AppDbContext _dbContext;

        public SubscriptionRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Subscription?> GetByIdAsync(Guid id)
         => await _dbContext.Subscriptions.FindAsync(id);
        public async Task<Subscription?> GetActiveByBusinessIdAsync(Guid businessId)
        => await _dbContext.Subscriptions
                .AsNoTracking()
                .FirstOrDefaultAsync(s =>
                    s.BusinessId == businessId && s.IsActive);
        public async Task<List<Subscription>> GetByBusinessIdAsync(Guid businessId)
        => await _dbContext.Subscriptions
                .AsNoTracking()
                .Where(s => s.BusinessId == businessId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        public async Task<List<Subscription>> GetPendingAsync()
        => await _dbContext.Subscriptions
                .AsNoTracking()
                .Where(s => !s.IsActive)
                .OrderBy(s => s.CreatedAt)
                .ToListAsync();

        public async Task<Subscription> CreateAsync(Subscription subscription)
        {
            await _dbContext.Subscriptions.AddAsync(subscription);
            await _dbContext.SaveChangesAsync();
            return subscription;
        }         

        public async Task<Subscription> UpdateAsync(Subscription subscription)
        {
            _dbContext.Subscriptions.Update(subscription);
            await _dbContext.SaveChangesAsync();
            return subscription;
        }
    }
}
