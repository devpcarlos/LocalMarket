

using LocalMarket.Core.DTos.Subscriptions;

namespace LocalMarket.Core.Interfaces
{
    public interface ISubscriptionService
    {
        Task<SubscriptionDto> RequestAsync(Guid userId, Guid businessId, CreateSubscriptionDto dto);
        Task<SubscriptionDto> ActivateAsync(Guid adminId, ActivateSubscriptionDto dto);
        Task<SubscriptionDto?> GetActiveByBusinessIdAsync(Guid businessId);
        Task<List<SubscriptionDto>> GetByBusinessIdAsync(Guid businessId);
        Task<List<SubscriptionDto>> GetPendingAsync();
    }
}
