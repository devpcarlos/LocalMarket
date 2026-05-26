
using System.ComponentModel.DataAnnotations;

namespace LocalMarket.Core.DTos.Favorites
{
   public class AddFavoriteDto
    {
        [Required]
        public Guid BusinessId { get; set; }
    }
}
