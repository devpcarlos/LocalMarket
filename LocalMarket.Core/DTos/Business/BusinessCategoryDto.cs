

namespace LocalMarket.Core.DTos.Business
{
    public class BusinessCategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Icon { get; set; }
    }
}
