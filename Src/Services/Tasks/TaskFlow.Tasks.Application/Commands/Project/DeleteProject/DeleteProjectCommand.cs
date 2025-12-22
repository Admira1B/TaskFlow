using MediatR;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Commands.TaskGroup.DeleteProject {
    public record DeleteProjectCommand(
        Guid Id
    ) : IRequest<RequestResult<Unit>>;
}
