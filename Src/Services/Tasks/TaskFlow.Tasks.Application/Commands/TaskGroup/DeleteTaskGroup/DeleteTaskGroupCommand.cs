using MediatR;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Commands.TaskGroup.DeleteTaskGroup {
    public record DeleteTaskGroupCommand(
        Guid Id
    ) : IRequest<RequestResult<Unit>>;
}
