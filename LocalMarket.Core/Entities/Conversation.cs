namespace LocalMarket.Core.Entities
{
    public class Conversation
    {
        public Guid Id { get; set; }
        public Guid BusinessId { get; set; }
        public Guid ClientId { get; set; }
        public DateTime? LastMessageAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
