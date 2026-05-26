using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LocalMarket.API.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected Guid GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)
                ?? User.FindFirst("sub")
                ?? throw new UnauthorizedAccessException("User not authenticated");
            return Guid.Parse(claim.Value);
        }
    }
}
