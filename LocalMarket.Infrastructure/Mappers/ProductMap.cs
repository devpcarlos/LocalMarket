using LocalMarket.Core.DTos.Products;
using LocalMarket.Core.Entities;
using Mapster;
namespace LocalMarket.Infrastructure.Mappers
{
    public class ProductMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // Product → ProductDto
            config.NewConfig<Product, ProductDto>();


            // CreateProductDto → Product
            config.NewConfig<RequestProductDto, Product>()
                .Map(dest => dest.IsAvailable, _ => true)
                .Map(dest => dest.CreatedAt, _ => DateTime.UtcNow)
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.BusinessId);

            // ProductCategory → ProductCategoryModel
            config.NewConfig<ProductCategory, ProductCategoryDto>();

            // CreateProductCategoryDto → ProductCategory
            config.NewConfig<CreateProductCategoryDto, ProductCategory>()
                .Map(dest => dest.CreatedAt, _ => DateTime.UtcNow)
                .Ignore(dest => dest.Id!)
                .Ignore(dest => dest.BusinessId);
        }
    }
}
