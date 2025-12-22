using MediatR;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Commands.TaskGroup.CreateTaskGroup {
    public record CreateTaskGroupCommand(
        Guid ProjectId,
        string Name
    ) : IRequest<RequestResult<Unit>>;
}
