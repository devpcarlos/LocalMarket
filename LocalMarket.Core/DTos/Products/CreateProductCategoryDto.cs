

using System.ComponentModel.DataAnnotations;

namespace LocalMarket.Core.DTos.Products
{
    public class CreateProductCategoryDto
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public int SortOrder { get; set; } = 0;
    }
}
