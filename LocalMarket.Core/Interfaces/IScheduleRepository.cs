using LocalMarket.Core.Entities;

namespace LocalMarket.Core.Interfaces
{
    public interface IScheduleRepository
    {
        Task<List<Schedule>> GetByBusinessIdAsync(Guid businessId);
        Task<Schedule?> GetByBusinessAndDayAsync(Guid businessId, int dayOfWeek);
        Task<Schedule> CreateAsync(Schedule schedule);
        Task<Schedule> UpdateAsync(Schedule schedule);
        Task DeleteByBusinessIdAsync(Guid businessId);
        Task SaveRangeAsync(List<Schedule> schedules);

    }
}
