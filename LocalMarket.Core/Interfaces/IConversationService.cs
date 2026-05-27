
using LocalMarket.Core.DTos.Conversations;
using LocalMarket.Core.DTos.Messages;

namespace LocalMarket.Core.Interfaces
{
    public interface IConversationService
    {
        Task<List<ConversationDto>> GetMyConversationsAsync(Guid userId);
        Task<List<ConversationDto>> GetBusinessConversationsAsync(Guid userId, Guid businessId);
        Task<List<MessageDto>> GetMessagesAsync(Guid userId, Guid conversationId);
        Task<MessageDto> SendMessageAsync(Guid senderId, Guid businessId, SendMessageDto dto);
        Task MarkAsReadAsync(Guid userId, Guid conversationId);
        Task<MessageDto> ReplyAsync(Guid ownerId, Guid conversationId, SendMessageDto dto);
    }
}
