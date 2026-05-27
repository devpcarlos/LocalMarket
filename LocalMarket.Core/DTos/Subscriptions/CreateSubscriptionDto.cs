

using System.ComponentModel.DataAnnotations;

namespace LocalMarket.Core.DTos.Subscriptions
{
    public class CreateSubscriptionDto
    {
        [Required]
        [RegularExpression("^(Basic|Standard|Premium)$",
            ErrorMessage = "Plan must be Basic, Standard or Premium")]
        public string Plan { get; set; } = string.Empty;

        [Required]
        public string PaymentDescription { get; set; } = string.Empty;
    }
}
