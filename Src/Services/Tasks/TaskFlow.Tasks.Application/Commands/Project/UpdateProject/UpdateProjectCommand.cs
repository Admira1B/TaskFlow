using MediatR;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Commands.TaskGroup.UpdateProject {
    public record UpdateProjectCommand(
        Guid Id, 
        string Name, 
        string? Description, 
        bool IsActive
    ) : IRequest<RequestResult<Unit>>;
}
