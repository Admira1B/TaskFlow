using MediatR;
using TaskFlow.Tasks.Domain.Enums;
using TaskFlow.Tasks.Application.Results;

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
