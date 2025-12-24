using MediatR;
using TaskFlow.Tasks.Application.DTOs;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Commands.Project.CreateProject {
    public record CreateProjectCommand(
        string Name,
        string? Description,
        Guid OwnerId
    ) : IRequest<RequestResult<ProjectDto>>;
}
