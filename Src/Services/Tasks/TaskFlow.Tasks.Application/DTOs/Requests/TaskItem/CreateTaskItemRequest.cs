using TaskFlow.Tasks.Domain.Enums;

namespace TaskFlow.Tasks.Application.DTOs.Requests.TaskItem {
    public record CreateTaskItemRequest(
        string Title,
        string? Description,
        Guid GroupId,
        Guid? AssignedId,
        Priority Priority
    );
}
