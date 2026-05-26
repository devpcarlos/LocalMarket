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
            // 1. Validaciones de negocio iniciales
            var business = await _businessRepository.GetByIdAsync(businessId)
               ?? throw new KeyNotFoundException($"Business {businessId} not found");

            if (business.UserId != userId)
                throw new UnauthorizedAccessException("You are not the owner of this business");

            // 🛡️ Validación de consistencia de horas (Cierre > Apertura)
            foreach (var s in dto.Schedules)
            {
                if (!s.IsClosed && s.ClosingTime <= s.OpeningTime)
                    throw new InvalidOperationException($"Closing time must be after opening time for day {s.DayOfWeek}");
            }

            // 2. Ejecutar operaciones en memoria (Preparar el lote)
            await _scheduleRepository.DeleteByBusinessIdAsync(businessId); // Pasó al estado "Deleted" en EF

            var schedulesEntities = dto.Schedules.Adapt<List<Schedule>>();
            foreach (var schedule in schedulesEntities)
            {
                schedule.BusinessId = businessId; // Asignar relación
            }
            await _scheduleRepository.SaveRangeAsync(schedulesEntities); // Pasó al estado "Added" en EF

            // 3. ENVIÁ EN UNA SOLA TRANSACCIÓN AUTOMÁTICA
            // Entity Framework envolverá el DeleteRange y el AddRange en un único bloque de SQL.
            // Si la inserción llegara a fallar por cualquier motivo, el borrado jamás ocurrirá en la BD.
            await _scheduleRepository.SaveChangesAsync();

            return schedulesEntities.Adapt<List<ScheduleDto>>();
        }
    }
}
