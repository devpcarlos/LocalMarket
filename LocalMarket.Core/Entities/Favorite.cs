

namespace LocalMarket.Core.Entities
{
    public class Favorite
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public Guid BusinessId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
