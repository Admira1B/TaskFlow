namespace TaskFlow.Tasks.Application.DTOs {
    public record ProjectDto(
        Guid Id,
        string Name,
        string? Description,
        Guid OwnerId,
        bool IsActive,
        DateTime CreatedAt,
        DateTime? UpdatedAt);
}
