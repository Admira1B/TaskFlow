using MediatR;

namespace TaskFlow.Tasks.Application.Commands.Project.DeleteProject {
    public record DeleteProjectCommand(
        Guid Id
    ) : IRequest<bool>;
}
