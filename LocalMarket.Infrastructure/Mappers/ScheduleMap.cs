

using LocalMarket.Core.DTos.Schedule;
using LocalMarket.Core.Entities;
using Mapster;

namespace LocalMarket.Infrastructure.Mappers
{
    public class ScheduleMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            //Scedule → ScheduleDto
            config.NewConfig<Schedule, ScheduleDto>()
                .Map(dt => dt.DayOfWeek, src => src.DayOfWeek)
                .Map(dt => dt.OpeningTime, src => src.OpeningTime ?? TimeOnly.MinValue)
                .Map(dt => dt.ClosingTime, src => src.ClosingTime ?? TimeOnly.MinValue)
                .Map(dt => dt.IsClosed, src => src.IsClosed);

            // ScheduleDto → Schedule -> creare o update
            config.NewConfig<RequestScheduleDto, Schedule>()
                .Ignore(dt => dt.Id)
                .Ignore(dt => dt.BusinessId)
                .Map(dt => dt.DayOfWeek, src => src.DayOfWeek)
                .Map(dt => dt.OpeningTime, src => src.OpeningTime ?? TimeOnly.MinValue)
                .Map(dt => dt.ClosingTime, src => src.ClosingTime ?? TimeOnly.MinValue)
                .Map(dt => dt.IsClosed, src => src.IsClosed);

        }
    }
}
