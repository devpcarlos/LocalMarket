namespace LocalMarket.Core.Entities
{
   public class Schedule
    {
        public Guid Id { get; set; }
        public Guid BusinessId { get; set; }
        public int DayOfWeek { get; set; }
        public TimeOnly? OpeningTime { get; set; }
        public TimeOnly? ClosingTime { get; set; }
        public bool IsClosed { get; set; } = false;
    }
}
