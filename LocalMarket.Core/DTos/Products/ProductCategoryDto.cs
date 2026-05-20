

namespace LocalMarket.Core.DTos.Products
{
    public class ProductCategoryDto
    {
        public Guid Id { get; set; }
        public Guid BusinessId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int SortOrder { get; set; }
    }
}
