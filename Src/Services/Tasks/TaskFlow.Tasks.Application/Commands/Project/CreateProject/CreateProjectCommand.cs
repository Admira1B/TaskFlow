using MediatR;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Contracts.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Commands.Project.CreateProject {
    public record CreateProjectCommand(
        string Name,
        string? Description,
        Guid OwnerId
    ) : IRequest<RequestResult<ProjectDto>>;
}
