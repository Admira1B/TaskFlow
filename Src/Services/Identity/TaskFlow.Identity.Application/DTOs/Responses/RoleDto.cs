namespace TaskFlow.Identity.Application.DTOs.Responses {
    public record RoleDto (
    Guid Id,
    string Name,
    string Description,
    DateTime CreatedAt,
    DateTime? UpdatedAt
    );
}
