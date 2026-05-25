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
            config.NewConfig<Product, ProductDto>()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.BusinessId)
                .Map(dest => dest.Name, src => src.Name.Trim())
                .Map(dest => dest.Description, src => src.Description != null 
                ? src.Description.Trim() : string.Empty)
                .Map(dest => dest.Price, src => src.Price)
                .Map(dest => dest.IsAvailable, src => src.IsAvailable);


            // CreateProductDto → Product
            config.NewConfig<RequestProductDto, Product>()
                .Map(dest => dest.IsAvailable, _ => true)
                .Map(dest => dest.CreatedAt, _ => DateTime.UtcNow)
                .Ignore(dest => dest.IsAvailable)
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.BusinessId);

            // UpdateProductDto → Product
            config.NewConfig<RequestProductDto, Product>()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.BusinessId)
                .Ignore(dest => dest.CreatedAt);

            // ProductCategory → ProductCategoryModel
            config.NewConfig<ProductCategory, ProductCategoryDto>();

            // CreateProductCategoryDto → ProductCategory
            config.NewConfig<CreateProductCategoryDto, ProductCategory>()
                .Map(dest => dest.CreatedAt, _ => DateTime.UtcNow)
                .Ignore(dest => dest.Id ?? Guid.Empty)
                .Ignore(dest => dest.BusinessId);
        }
    }
}
