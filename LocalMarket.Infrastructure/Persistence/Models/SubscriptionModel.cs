using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace LocalMarket.Infrastructure.Persistence.Models
{
    public class SubscriptionModel : BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; }

        [Column("business_id")]
        public Guid BusinessId { get; set; }

        [Column("plan")]
        public string Plan { get; set; } = string.Empty;

        [Column("monthly_price")]
        public decimal MonthlyPrice { get; set; }

        [Column("start_date")]
        public DateOnly StartDate { get; set; }

        [Column("end_date")]
        public DateOnly EndDate { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("payment_description")]
        public string? PaymentDescription { get; set; }

        [Column("activated_by")]
        public Guid? ActivatedBy { get; set; }

        [Column("activated_at")]
        public DateTime? ActivatedAt { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
