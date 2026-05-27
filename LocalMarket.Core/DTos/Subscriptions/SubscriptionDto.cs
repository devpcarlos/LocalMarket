
namespace LocalMarket.Core.DTos.Subscriptions
{
    public class SubscriptionDto
    {
        public Guid Id { get; set; }
        public Guid BusinessId { get; set; }
        public string Plan { get; set; } = string.Empty;
        public decimal MonthlyPrice { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public bool IsActive { get; set; }
        public string? PaymentDescription { get; set; }
        public DateTime? ActivatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
