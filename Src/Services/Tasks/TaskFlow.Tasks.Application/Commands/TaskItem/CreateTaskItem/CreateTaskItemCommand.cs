using MediatR;
using TaskFlow.Tasks.Application.DTOs;
using TaskFlow.Tasks.Domain.Enums;

namespace TaskFlow.Tasks.Application.Commands.TaskItem.CreateTaskItem {
    public record CreateTaskItemCommand(
        string Title,
        string? Description, 
        Guid GroupId, 
        Guid ReporterId, 
        Guid? AssignedId, 
        Priority Priority
    ) : IRequest<TaskItemDto?>;
}
