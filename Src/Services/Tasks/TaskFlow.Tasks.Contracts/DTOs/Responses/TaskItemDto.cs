using TaskFlow.Tasks.Domain.Enums;

namespace TaskFlow.Tasks.Contracts.DTOs.Responses {
    public record TaskItemDto(
        Guid Id,
        string Title,
        string? Description,
        Guid ProjectId,
        Guid GroupId,
        Guid ReporterId,
        Guid? AssignedId,
        Priority Priority,
        DateTime CreatedAt,
        DateTime? UpdatedAt);
}
