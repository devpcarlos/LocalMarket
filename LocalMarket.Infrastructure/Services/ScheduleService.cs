using LocalMarket.Core.DTos.Schedule;
using LocalMarket.Core.Entities;
using LocalMarket.Core.Interfaces;
using Mapster;

namespace LocalMarket.Infrastructure.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IBusinessRepository _businessRepository;

        public ScheduleService(
            IScheduleRepository scheduleRepository,
            IBusinessRepository businessRepository)
        {
            _scheduleRepository = scheduleRepository;
            _businessRepository = businessRepository;
        }

        public async Task<List<ScheduleDto>> GetByBusinessIdAsync(Guid businessId)
        {
            var schedules = await _scheduleRepository.GetByBusinessIdAsync(businessId);
            return schedules.Adapt<List<ScheduleDto>>();
        }

        public async Task<List<ScheduleDto>> UpsertAsync(Guid userId, Guid businessId, ScheduleListDto dto)
        {
            var business = await _businessRepository.GetByIdAsync(businessId)
               ?? throw new KeyNotFoundException($"Business {businessId} not found");

            if (business.UserId != userId)
                throw new UnauthorizedAccessException(
                    "You are not the owner of this business");

            // Reemplazar todos los horarios del negocio
            await _scheduleRepository.DeleteByBusinessIdAsync(businessId);

            var schedule = dto.Schedules.Adapt<List<Schedule>>();

            //Asignar el BusinessId a cada horario mapeado
            foreach (var schedules in schedule)
            {
                schedules.BusinessId = businessId;
            }

            await _scheduleRepository.SaveRangeAsync(schedule);

            return schedule.Adapt<List<ScheduleDto>>();
        }
    }
}
