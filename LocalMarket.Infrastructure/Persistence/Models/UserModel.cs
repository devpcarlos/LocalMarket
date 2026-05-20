using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace LocalMarket.Infrastructure.Persistence.Models
{
    [Table("users")]
    public class UserModel : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("phone")]
        public string? Phone { get; set; }

        [Column("role")]
        public string Role { get; set; } = "client";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
