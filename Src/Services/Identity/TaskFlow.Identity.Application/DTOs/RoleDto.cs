namespace TaskFlow.Identity.Application.DTOs {
    public record RoleDto (
    Guid Id,
    string Name,
    string Description,
    DateTime CreatedAt,
    DateTime? UpdatedAt
    );
}
