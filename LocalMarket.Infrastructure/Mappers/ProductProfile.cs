using LocalMarket.Core.DTos.Products;
using LocalMarket.Core.Entities;
using LocalMarket.Infrastructure.Persistence.Models;
using Mapster;
namespace LocalMarket.Infrastructure.Mappers
{
    public class ProductProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // ProductModel → Product
            config.NewConfig<ProductModel, Product>();

            // Product → ProductModel
            config.NewConfig<Product, ProductModel>();

            // Product → ProductDto
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

            // ProductCategoryModel → ProductCategory
            config.NewConfig<ProductCategoryModel, ProductCategory>();

            // ProductCategory → ProductCategoryModel
            config.NewConfig<ProductCategory, ProductCategoryModel>();

            // ProductCategory → ProductCategoryDto
            config.NewConfig<ProductCategory, ProductCategoryDto>();

            // CreateProductCategoryDto → ProductCategory
            config.NewConfig<CreateProductCategoryDto, ProductCategory>()
                .Map(dest => dest.CreatedAt, _ => DateTime.UtcNow)
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.BusinessId);
        }
    }
}
