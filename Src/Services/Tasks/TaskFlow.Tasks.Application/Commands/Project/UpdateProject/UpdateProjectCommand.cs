using MediatR;
using TaskFlow.Shared.Core.Results;

namespace TaskFlow.Tasks.Application.Commands.Project.UpdateProject {
    public record UpdateProjectCommand(
        Guid Id, 
        string Name, 
        string? Description, 
        bool IsActive
    ) : IRequest<RequestResult<Unit>>;
}
