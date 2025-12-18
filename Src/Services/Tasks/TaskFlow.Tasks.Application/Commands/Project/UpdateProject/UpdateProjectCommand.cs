using MediatR;

namespace TaskFlow.Tasks.Application.Commands.Project.UpdateProject {
    public record UpdateProjectCommand(
        Guid Id, 
        string Name, 
        string? Description, 
        bool IsActive
    ) : IRequest<bool>;
}
