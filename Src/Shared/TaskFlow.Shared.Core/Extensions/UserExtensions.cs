using System.Security.Claims;

namespace TaskFlow.Shared.Core.Extensions {
    public static class UserExtensions {
        public static Guid GetUserId(this ClaimsPrincipal user) {
            return Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }
    }
}
