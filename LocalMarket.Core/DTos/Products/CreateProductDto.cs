
using System.ComponentModel.DataAnnotations;

namespace LocalMarket.Core.DTos.Products
{
    public class CreateProductDto
    {
        public Guid? ProductCategoryId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, 999999999, ErrorMessage = "Price must be a positive value")]
        public decimal Price { get; set; }

        [Range(0, 999999999, ErrorMessage = "Sale price must be a positive value")]
        public decimal? SalePrice { get; set; }

        public List<string> PhotoUrls { get; set; } = [];
    }
}
