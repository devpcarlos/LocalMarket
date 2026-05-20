using LocalMarket.Core.Entities;
using LocalMarket.Core.Interfaces;
using LocalMarket.Infrastructure.Persistence.Models;
using Mapster;

namespace LocalMarket.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly Supabase.Client _supabase;

        public ProductRepository(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<List<Product>> GetByBusinessIdAsync(Guid businessId)
        {
            var result = await _supabase
                .From<ProductModel>()
                .Where(p => p.BusinessId == businessId)
                .Get();

            return result.Models.Adapt<List<Product>>();
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            var result = await _supabase
                .From<ProductModel>()
                .Where(p => p.Id == id)
                .Single();

            return result?.Adapt<Product>();
        }

        public async Task<Product> CreateAsync(Product product)
        {
            var model = product.Adapt<ProductModel>();
            var result = await _supabase
                .From<ProductModel>()
                .Insert(model);

            return result.Models.First().Adapt<Product>();
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            var model = product.Adapt<ProductModel>();
            var result = await _supabase
                .From<ProductModel>()
                .Where(p => p.Id == product.Id)
                .Update(model);

            return result.Models.First().Adapt<Product>();
        }

        public async Task DeleteAsync(Guid id)
        {
            await _supabase
                .From<ProductModel>()
                .Where(p => p.Id == id)
                .Delete();
        }
    }
}
