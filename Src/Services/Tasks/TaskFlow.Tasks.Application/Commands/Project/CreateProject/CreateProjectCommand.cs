using MediatR;
using TaskFlow.Tasks.Application.DTOs;

namespace TaskFlow.Tasks.Application.Commands.Project.CreateProject {
    public record CreateProjectCommand(
        string Name,
        string? Description,
        Guid OwnerId
    ) : IRequest<ProjectDto?>;
}
