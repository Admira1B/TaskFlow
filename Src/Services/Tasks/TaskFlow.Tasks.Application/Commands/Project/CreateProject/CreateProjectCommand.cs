using MediatR;
using TaskFlow.Tasks.Application.DTOs.Responses;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Commands.Project.CreateProject {
    public record CreateProjectCommand(
        string Name,
        string? Description,
        Guid OwnerId
    ) : IRequest<RequestResult<ProjectDto>>;
}
