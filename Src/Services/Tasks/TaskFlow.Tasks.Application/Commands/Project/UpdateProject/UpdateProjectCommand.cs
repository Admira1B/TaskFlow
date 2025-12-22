using MediatR;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Commands.Project.UpdateProject {
    public record UpdateProjectCommand(
        Guid Id, 
        string Name, 
        string? Description, 
        bool IsActive
    ) : IRequest<RequestResult<Unit>>;
}
