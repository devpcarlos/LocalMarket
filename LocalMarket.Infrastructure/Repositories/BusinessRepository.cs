using LocalMarket.Core.Entities;
using LocalMarket.Core.Interfaces;
using LocalMarket.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LocalMarket.Infrastructure.Repositories
{
    public class BusinessRepository : IBusinessRepository
    
    {
        private readonly AppDbContext _dbContext;
        public BusinessRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Business>> GetAllAsync(
            Guid? categoryId, string? city, string? search)
        {
            var query = _dbContext.Businesses
                .AsNoTracking().Where(b=>b.IsActive);

            if (categoryId.HasValue && categoryId.Value != Guid.Empty) {
                query = query.Where(b => b.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(city))
                query = query.Where(b => b.City == city);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var safeSearch = search
                    .Replace("\\", "\\\\")
                    .Replace("%", "\\%");

                query = query.Where(b =>
                    EF.Functions.Like(b.Name, $"%{safeSearch}%", "\\"));
            }

            return await query.ToListAsync();
        }

        public async Task<Business?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Businesses.FindAsync(id);
        }

        public async Task<Business?> GetByUserIdAsync(Guid userId)
        {
            return await _dbContext.Businesses
                .FirstOrDefaultAsync(b => b.UserId == userId);
        }

        public async Task<Business> CreateAsync(Business business)
        {
            await _dbContext.Businesses.AddAsync(business);
            await _dbContext.SaveChangesAsync();
            return business;
        }

        public async Task<Business> UpdateAsync(Business business)
        {
            _dbContext.Businesses.Update(business);
            await _dbContext.SaveChangesAsync();
            return business;
        }

        public async Task DeleteAsync(Business business)
        {
            _dbContext.Businesses.Remove(business);
                await _dbContext.SaveChangesAsync();
            
        }

    }
}
