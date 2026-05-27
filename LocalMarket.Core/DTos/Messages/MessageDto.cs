
namespace LocalMarket.Core.DTos.Messages
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
        public Guid SenderId { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Type { get; set; } = "text";
        public bool IsRead { get; set; }
        public DateTime SentAt { get; set; }
    }
}
