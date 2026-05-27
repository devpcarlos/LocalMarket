

using LocalMarket.Core.Entities;

namespace LocalMarket.Core.Interfaces
{
    public interface IConversationRepository
    {
        Task<Conversation?> GetByIdAsync(Guid id);
        Task<Conversation?> GetByBusinessAndClientAsync(Guid businessId, Guid clientId);
        Task<List<Conversation>> GetByUserIdAsync(Guid userId);
        Task<List<Conversation>> GetByBusinessIdAsync(Guid businessId);
        Task<Conversation> CreateAsync(Conversation conversation);
        Task UpdateLastMessageAtAsync(Guid conversationId, DateTime sentAt);
    }
}
