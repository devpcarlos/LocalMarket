using LocalMarket.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocalMarket.Core.Entities
{
    public class Subscription
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid BusinessId { get; set; }
        public SubscriptionPlan Plan { get; set; }
        public decimal MonthlyPrice { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public bool IsActive { get; set; } = false;
        public string? PaymentDescription { get; set; }
        public Guid? ActivatedBy { get; set; }
        public DateTime? ActivatedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
