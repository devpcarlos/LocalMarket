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
            string? categoryId, string? city, string? search)
        {
            var query = _dbContext.Businesses.Where(b=>b.IsActive);

            if (!string.IsNullOrWhiteSpace(categoryId) &&
                Guid.TryParse(categoryId, out var catGuid))
                query = query.Where(b => b.CategoryId == catGuid);

            if (!string.IsNullOrWhiteSpace(city))
                query = query.Where(b => b.City == city);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(b => b.Name.ToLower().Contains(searchLower));
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
            return business;
        }

        public async Task DeleteAsync(Guid id)
        {
            var business = await _dbContext.Businesses.FindAsync(id);
            if (business != null)
            {
                _dbContext.Businesses.Remove(business);
                await _dbContext.SaveChangesAsync();
            }
        }

    }
}
