using MediatR;
using TaskFlow.Shared.Core.Results;

namespace TaskFlow.Tasks.Application.Commands.TaskGroup.UpdateTaskGroup {
    public record UpdateTaskGroupCommand(
        Guid Id,
        string Name
    ) : IRequest<RequestResult<Unit>>;
}
