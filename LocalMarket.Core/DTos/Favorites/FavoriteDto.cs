

namespace LocalMarket.Core.DTos.Favorites
{
    public class FavoriteDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid BusinessId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
