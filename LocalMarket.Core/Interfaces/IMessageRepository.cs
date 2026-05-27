

using LocalMarket.Core.Entities;

namespace LocalMarket.Core.Interfaces
{
    public interface IMessageRepository
    {
        Task<List<Message>> GetByConversationIdAsync(Guid conversationId);
        Task<Message> CreateAsync(Message message);
        Task MarkAsReadAsync(Guid conversationId, Guid readerId);
    }
}
