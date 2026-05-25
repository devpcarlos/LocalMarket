
using System.ComponentModel.DataAnnotations;

namespace LocalMarket.Core.DTos.Schedule
{
   public class ScheduleListDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "At least one schedule is required")]
        public List<RequestScheduleDto> Schedules { get; set; } = [];
    }
}
