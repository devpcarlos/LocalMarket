using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace LocalMarket.Infrastructure.Persistence.Models
{
    [Table("products")]
    public class ProductModel : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; }

        [Column("business_id")]
        public Guid BusinessId { get; set; }

        [Column("product_category_id")]
        public Guid? ProductCategoryId { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("description")]
        public string? Description { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("sale_price")]
        public decimal? SalePrice { get; set; }

        [Column("photo_urls")]
        public List<string> PhotoUrls { get; set; } = [];

        [Column("is_available")]
        public bool IsAvailable { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
