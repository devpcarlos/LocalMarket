using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;


namespace LocalMarket.Infrastructure.Persistence.Models
{
    [Table("conversations")]
    public class ConversationModel : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; }

        [Column("business_id")]
        public Guid BusinessId { get; set; }

        [Column("client_id")]
        public Guid ClientId { get; set; }

        [Column("last_message_at")]
        public DateTime? LastMessageAt { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
