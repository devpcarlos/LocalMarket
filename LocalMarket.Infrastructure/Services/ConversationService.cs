

using LocalMarket.Core.DTos.Conversations;
using LocalMarket.Core.DTos.Messages;
using LocalMarket.Core.Entities;
using LocalMarket.Core.Interfaces;
using Mapster;

namespace LocalMarket.Infrastructure.Services
{
    public class ConversationService : IConversationService
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IBusinessRepository _businessRepository;

        public ConversationService(
            IConversationRepository conversationRepository,
            IMessageRepository messageRepository,
            IBusinessRepository businessRepository)
        {
            _conversationRepository = conversationRepository;
            _messageRepository = messageRepository;
            _businessRepository = businessRepository;
        }
        public async Task<List<ConversationDto>> GetMyConversationsAsync(Guid userId)
        {
            var conversations = await _conversationRepository.GetByUserIdAsync(userId);
            return conversations.Adapt<List<ConversationDto>>();
        }
        public async Task<List<ConversationDto>> GetBusinessConversationsAsync(Guid userId, Guid businessId)
        {
            var business = await _businessRepository.GetByIdAsync(businessId)
                ?? throw new KeyNotFoundException($"Business {businessId} not found");

            if (business.UserId != userId)
                throw new UnauthorizedAccessException(
                    "You are not the owner of this business");

            var conversations = await _conversationRepository
                .GetByBusinessIdAsync(businessId);

            return conversations.Adapt<List<ConversationDto>>();
        }

        public async Task<List<MessageDto>> GetMessagesAsync(Guid userId, Guid conversationId)
        {
            var conversation = await _conversationRepository.GetByIdAsync(conversationId)
                ?? throw new KeyNotFoundException(
                    $"Conversation {conversationId} not found");

            await ValidateParticipantAsync(userId, conversation);

            var messages = await _messageRepository
                .GetByConversationIdAsync(conversationId);

            return messages.Adapt<List<MessageDto>>();
        }
        public async Task<MessageDto> SendMessageAsync(Guid senderId, Guid businessId, SendMessageDto dto)
        {
            _ = await _businessRepository.GetByIdAsync(businessId)
               ?? throw new KeyNotFoundException($"Business {businessId} not found");

            // Obtener o crear conversación
            var conversation = await _conversationRepository
                .GetByBusinessAndClientAsync(businessId, senderId);

            if (conversation is null)
            {
                conversation = new Conversation
                {
                    Id = Guid.NewGuid(),
                    BusinessId = businessId,
                    ClientId = senderId,
                    CreatedAt = DateTime.UtcNow
                };
                conversation = await _conversationRepository.CreateAsync(conversation);
            }

            var message = dto.Adapt<Message>();
            message.Id = Guid.NewGuid();
            message.ConversationId = conversation.Id;
            message.SenderId = senderId;

            var created = await _messageRepository.CreateAsync(message);

            await _conversationRepository
                .UpdateLastMessageAtAsync(conversation.Id, created.SentAt);

            return created.Adapt<MessageDto>();
        }
        public async Task MarkAsReadAsync(Guid userId, Guid conversationId)
        {
            var conversation = await _conversationRepository.GetByIdAsync(conversationId)
                ?? throw new KeyNotFoundException(
                    $"Conversation {conversationId} not found");

            await ValidateParticipantAsync(userId, conversation);

            await _messageRepository.MarkAsReadAsync(conversationId, userId);
        }

        // ── Helpers ────────────────────────────────────────────────────────
        private async Task ValidateParticipantAsync(Guid userId, Conversation conversation)
        {
            var business = await _businessRepository
                .GetByIdAsync(conversation.BusinessId);

            var isClient = conversation.ClientId == userId;
            var isOwner = business?.UserId == userId;

            if (!isClient && !isOwner)
                throw new UnauthorizedAccessException(
                    "You are not a participant of this conversation");
        }

        public async Task<MessageDto> ReplyAsync(Guid ownerId, Guid conversationId, SendMessageDto dto)
        {
            var conversation = await _conversationRepository.GetByIdAsync(conversationId)
         ?? throw new KeyNotFoundException(
             $"Conversation {conversationId} not found");

            var business = await _businessRepository.GetByIdAsync(conversation.BusinessId)
                ?? throw new KeyNotFoundException("Business not found");

            if (business.UserId != ownerId)
                throw new UnauthorizedAccessException(
                    "You are not the owner of this business");

            var message = dto.Adapt<Message>();
            message.Id = Guid.NewGuid();
            message.ConversationId = conversationId;
            message.SenderId = ownerId;

            var created = await _messageRepository.CreateAsync(message);

            await _conversationRepository
                .UpdateLastMessageAtAsync(conversationId, created.SentAt);

            return created.Adapt<MessageDto>();
        }
    }

 }

