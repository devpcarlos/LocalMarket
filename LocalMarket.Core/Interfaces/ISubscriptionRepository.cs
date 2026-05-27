
using LocalMarket.Core.Entities;

namespace LocalMarket.Core.Interfaces
{
    public interface ISubscriptionRepository
    {
        Task<Subscription?> GetByIdAsync(Guid id);
        Task<Subscription?> GetActiveByBusinessIdAsync(Guid businessId);
        Task<List<Subscription>> GetByBusinessIdAsync(Guid businessId);
        Task<List<Subscription>> GetPendingAsync();
        Task<Subscription> CreateAsync(Subscription subscription);
        Task<Subscription> UpdateAsync(Subscription subscription);
    }
}
