using MediatR;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Commands.TaskGroup.DeleteTaskGroup {
    public record DeleteTaskGroupCommand(
        Guid GroupId
    ) : IRequest<RequestResult<Unit>>;
}
