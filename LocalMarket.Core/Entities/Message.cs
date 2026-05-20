namespace LocalMarket.Core.Entities
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
        public Guid SenderId { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Type { get; set; } = "text";
        public bool IsRead { get; set; } = false;
        public DateTime SentAt { get; set; }
    }
}
