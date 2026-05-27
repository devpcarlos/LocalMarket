

using System.ComponentModel.DataAnnotations;

namespace LocalMarket.Core.DTos.Subscriptions
{
    public class ActivateSubscriptionDto
    {
        [Required]
        public Guid SubscriptionId { get; set; }
    }
}
