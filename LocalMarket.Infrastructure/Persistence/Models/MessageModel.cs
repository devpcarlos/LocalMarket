using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace LocalMarket.Infrastructure.Persistence.Models
{
    [Table("messages")]
    public class MessageModel : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; }

        [Column("conversation_id")]
        public Guid ConversationId { get; set; }

        [Column("sender_id")]
        public Guid SenderId { get; set; }

        [Column("content")]
        public string Content { get; set; } = string.Empty;

        [Column("type")]
        public string Type { get; set; } = "text";

        [Column("is_read")]
        public bool IsRead { get; set; }

        [Column("sent_at")]
        public DateTime SentAt { get; set; }
    }
}
