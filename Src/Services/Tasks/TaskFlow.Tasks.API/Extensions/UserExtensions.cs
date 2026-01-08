using System.Security.Claims;

namespace TaskFlow.Tasks.API.Extensions {
    public static class UserExtensions {
        public static Guid GetUserId(this ClaimsPrincipal user) {
            return Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }
    }
}
