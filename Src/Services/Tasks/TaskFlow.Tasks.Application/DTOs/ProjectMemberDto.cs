using TaskFlow.Tasks.Domain.Enums;

namespace TaskFlow.Tasks.Application.DTOs {
    public record ProjectMemberDto(
        Guid Id,
        Guid UserId,
        Guid ProjectId,
        ProjectRole Role,
        DateTime CreatedAt,
        DateTime? UpdatedAt);
}
