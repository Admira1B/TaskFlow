using MediatR;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Commands.TaskGroup.UpdateTaskGroup {
    public record UpdateTaskGroupCommand(
        Guid Id,
        string Name
    ) : IRequest<RequestResult<Unit>>;
}
