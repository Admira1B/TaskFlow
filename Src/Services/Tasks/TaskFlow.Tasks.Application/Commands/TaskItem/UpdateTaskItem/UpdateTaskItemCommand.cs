using MediatR;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Enums;

namespace TaskFlow.Tasks.Application.Commands.TaskItem.UpdateTaskItem {
    public record UpdateTaskItemCommand(
        Guid Id, 
        string Title, 
        string? Description, 
        Guid? AssignedId, 
        Priority Priority,
        Guid GroupId
    ) : IRequest<RequestResult<Unit>>;
}
