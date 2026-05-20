
namespace LocalMarket.Core.DTos.Products
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public Guid BusinessId { get; set; }
        public Guid? ProductCategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal? SalePrice { get; set; }
        public List<string> PhotoUrls { get; set; } = [];
        public bool IsAvailable { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
