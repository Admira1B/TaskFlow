using MediatR;
using TaskFlow.Shared.Core.Results;

namespace TaskFlow.Tasks.Application.Commands.Project.DeleteProject {
    public record DeleteProjectCommand(
        Guid Id
    ) : IRequest<RequestResult<Unit>>;
}
