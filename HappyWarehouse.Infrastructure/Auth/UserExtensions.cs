using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HappyWarehouse.Infrastructure.Auth
{
    public static class UserExtensions
    {
        public static int? GetUserId(this ClaimsPrincipal user)
        {
            var v =
                user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ??
                user.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                user.FindFirst("uid")?.Value;

            return int.TryParse(v, out var id) ? id : (int?)null;
        }
    }
}
