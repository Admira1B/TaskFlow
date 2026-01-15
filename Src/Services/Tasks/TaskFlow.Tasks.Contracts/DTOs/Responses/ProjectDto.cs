namespace TaskFlow.Tasks.Contracts.DTOs.Responses {
    public record ProjectDto(
        Guid Id,
        string Name,
        string? Description,
        Guid OwnerId,
        bool IsActive,
        DateTime CreatedAt,
        DateTime? UpdatedAt);
}
