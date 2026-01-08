using TaskFlow.Tasks.Domain.Enums;

namespace TaskFlow.Tasks.Application.DTOs.Requests.TaskItem {
    public record UpdateTaskItemRequest(
        Guid Id,
        string Title,
        string? Description,
        Guid? AssignedId,
        Priority Priority,
        Guid GroupId
    );
}
