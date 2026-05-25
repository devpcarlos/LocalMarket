
using System.ComponentModel.DataAnnotations;

namespace LocalMarket.Core.DTos.Schedule
{
    public class RequestScheduleDto
    {
        [Range(1, 7, ErrorMessage = "DayOfWeek must be between 1 and 7")]
        public int DayOfWeek { get; set; }

        public TimeOnly? OpeningTime { get; set; }
        public TimeOnly? ClosingTime { get; set; }
        public bool IsClosed { get; set; } = false;
    }
}
