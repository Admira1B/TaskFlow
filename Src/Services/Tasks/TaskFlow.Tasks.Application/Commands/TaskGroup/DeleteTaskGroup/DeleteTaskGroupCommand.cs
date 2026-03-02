using MediatR;
using TaskFlow.Shared.Core.Results;

namespace TaskFlow.Tasks.Application.Commands.TaskGroup.DeleteTaskGroup {
    public record DeleteTaskGroupCommand(
        Guid Id
    ) : IRequest<RequestResult<Unit>>;
}
