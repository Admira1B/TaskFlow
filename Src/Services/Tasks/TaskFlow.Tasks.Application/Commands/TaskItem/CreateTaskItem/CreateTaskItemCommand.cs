using MediatR;
using TaskFlow.Shared.Core.Results;
using TaskFlow.Tasks.Domain.Enums;
using TaskFlow.Tasks.Contracts.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Commands.TaskItem.CreateTaskItem {
    public record CreateTaskItemCommand(
        string Title,
        string? Description, 
        Guid GroupId, 
        Guid ReporterId, 
        Guid? AssignedId, 
        Priority Priority
    ) : IRequest<RequestResult<TaskItemDto>>;
}
