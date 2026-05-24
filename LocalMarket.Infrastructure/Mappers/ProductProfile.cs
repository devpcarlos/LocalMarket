using LocalMarket.Core.DTos.Products;
using LocalMarket.Core.Entities;
using Mapster;
namespace LocalMarket.Infrastructure.Mappers
{
    public class ProductProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // Product → ProductModel
            config.NewConfig<Product, ProductDto>();

            // CreateProductDto → Product
            config.NewConfig<CreateProductDto, Product>()
                .Map(dest => dest.IsAvailable, _ => true)
                .Map(dest => dest.CreatedAt, _ => DateTime.UtcNow)
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.BusinessId);

            // UpdateProductDto → Product
            config.NewConfig<UpdateProductDto, Product>()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.BusinessId)
                .Ignore(dest => dest.CreatedAt);

            // ProductCategory → ProductCategoryModel
            config.NewConfig<ProductCategory, ProductCategoryDto>();

            // CreateProductCategoryDto → ProductCategory
            config.NewConfig<CreateProductCategoryDto, ProductCategory>()
                .Map(dest => dest.CreatedAt, _ => DateTime.UtcNow)
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.BusinessId);
        }
    }
}
