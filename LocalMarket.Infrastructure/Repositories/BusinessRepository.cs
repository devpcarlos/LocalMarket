using LocalMarket.Core.Entities;
using LocalMarket.Core.Interfaces;
using LocalMarket.Infrastructure.Persistence.Models;
using Mapster;

namespace LocalMarket.Infrastructure.Repositories
{
    public class BusinessRepository : IBusinessRepository
    
    {
        private readonly Supabase.Client _supabase;

        public BusinessRepository(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<List<Business>> GetAllAsync(
            string? categoryId, string? city, string? search)
        {
            var query = _supabase
                .From<BusinessModel>()
                .Where(b => b.IsActive == true && b.IsVerified == true);

            if (!string.IsNullOrWhiteSpace(categoryId) &&
                Guid.TryParse(categoryId, out var catGuid))
                query = query.Where(b => b.CategoryId == catGuid);

            if (!string.IsNullOrWhiteSpace(city))
                query = query.Where(b => b.City == city);

            var result = await query.Get();
            var businesses = result.Models;

            if (!string.IsNullOrWhiteSpace(search))
                businesses = businesses
                    .Where(b => b.Name.Contains(search,
                        StringComparison.OrdinalIgnoreCase))
                    .ToList();

            return businesses.Adapt<List<Business>>();
        }

        public async Task<Business?> GetByIdAsync(Guid id)
        {
            var result = await _supabase
                .From<BusinessModel>()
                .Where(b => b.Id == id)
                .Single();

            return result?.Adapt<Business>();
        }

        public async Task<Business?> GetByUserIdAsync(Guid userId)
        {
            var result = await _supabase
                .From<BusinessModel>()
                .Where(b => b.UserId == userId)
                .Single();

            return result?.Adapt<Business>();
        }

        public async Task<Business> CreateAsync(Business business)
        {
            var model = business.Adapt<BusinessModel>();
            var result = await _supabase
                .From<BusinessModel>()
                .Insert(model);

            return result.Models.First().Adapt<Business>();
        }

        public async Task<Business> UpdateAsync(Business business)
        {
            var model = business.Adapt<BusinessModel>();
            var result = await _supabase
                .From<BusinessModel>()
                .Where(b => b.Id == business.Id)
                .Update(model);

            return result.Models.First().Adapt<Business>();
        }

        public async Task DeleteAsync(Guid id)
        {
            await _supabase
                .From<BusinessModel>()
                .Where(b => b.Id == id)
                .Delete();
        }

    }
}
