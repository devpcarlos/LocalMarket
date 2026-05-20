using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

namespace LocalMarket.Infrastructure.Persistence.Models
{

    [Table("businesses")]
    public class BusinessModel : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("category_id")]
        public Guid CategoryId { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("description")]
        public string? Description { get; set; }

        [Column("nit")]
        public string? Nit { get; set; }

        [Column("address")]
        public string? Address { get; set; }

        [Column("city")]
        public string? City { get; set; }

        [Column("phone")]
        public string? Phone { get; set; }

        [Column("has_whatsapp")]
        public bool HasWhatsapp { get; set; }

        [Column("logo_url")]
        public string? LogoUrl { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("is_verified")]
        public bool IsVerified { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
