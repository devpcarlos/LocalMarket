

using LocalMarket.Core.DTos.Conversations;
using LocalMarket.Core.DTos.Messages;
using LocalMarket.Core.Entities;
using Mapster;

namespace LocalMarket.Infrastructure.Mappers
{
    public class ConversationMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Conversation, ConversationDto>();

            config.NewConfig<Message, MessageDto>();

            config.NewConfig<SendMessageDto, Message>()
                .Map(dest => dest.Content, src => src.Content.Trim())
                .Map(dest => dest.Type, src => src.Type)
                .Map(dest => dest.SentAt, _ => DateTime.UtcNow)
                .Map(dest => dest.IsRead, _ => false)
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.ConversationId)
                .Ignore(dest => dest.SenderId);
        }
    }
}
