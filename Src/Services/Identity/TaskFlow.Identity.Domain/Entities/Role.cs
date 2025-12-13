using Microsoft.AspNetCore.Identity;

namespace TaskFlow.Identity.Domain.Entities {
    public class Role : IdentityRole<Guid> {
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
