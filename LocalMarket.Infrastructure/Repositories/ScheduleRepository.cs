using LocalMarket.Core.Entities;
using LocalMarket.Core.Interfaces;
using LocalMarket.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LocalMarket.Infrastructure.Repositories
{
   public class ScheduleRepository : IScheduleRepository
    {
        private readonly AppDbContext _dbContext;

        public ScheduleRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Schedule>> GetByBusinessIdAsync(Guid businessId)
        {
            return await _dbContext.Schedules
                .Where(s => s.BusinessId == businessId)
                .OrderBy(s => s.DayOfWeek).ToListAsync();
        }

        public async Task<Schedule?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Schedules.FindAsync(id);
        }

        public async Task<Schedule?> GetByBusinessAndDayAsync(Guid businessId, int dayOfWeek)
        {
            return await _dbContext.Schedules
                .FirstOrDefaultAsync(s => s.BusinessId == businessId && s.DayOfWeek == dayOfWeek);
        }

        public async Task<Schedule> CreateAsync(Schedule schedule)
        {
            await _dbContext.Schedules.AddAsync(schedule);
            await _dbContext.SaveChangesAsync();
            return schedule;
        }

        public async Task<Schedule> UpdateAsync(Schedule schedule)
        {
            _dbContext.Schedules.Update(schedule);
            await _dbContext.SaveChangesAsync();
            return schedule;
        }

        public async Task DeleteByBusinessIdAsync(Guid businessId)
        {
            var schedules = await _dbContext.Schedules
                .Where(s => s.BusinessId == businessId)
                .ToListAsync();

            _dbContext.Schedules.RemoveRange(schedules);
        }

        public async Task SaveRangeAsync(List<Schedule> schedules)
        {
            await _dbContext.Schedules.AddRangeAsync(schedules);
        }
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
