
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace LocalMarket.Infrastructure.Persistence.Models
{
    [Table("product_categories")]
    public class ProductCategoryModel : BaseModel
    {

        [PrimaryKey("id", false)]
        public Guid Id { get; set; }

        [Column("business_id")]
        public Guid BusinessId { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("sort_order")]
        public int SortOrder { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

    }
}
