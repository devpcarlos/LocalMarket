
using LocalMarket.Core.DTos.Schedule;

namespace LocalMarket.Core.Interfaces
{
    public interface IScheduleService
    {
        Task<List<ScheduleDto>> GetByBusinessIdAsync(Guid businessId);
        Task<List<ScheduleDto>> UpsertAsync(Guid userId, Guid businessId, ScheduleListDto dto);
    }
}
