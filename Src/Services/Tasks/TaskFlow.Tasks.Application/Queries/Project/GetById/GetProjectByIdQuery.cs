using MediatR;
using TaskFlow.Tasks.Application.DTOs;

namespace TaskFlow.Tasks.Application.Queries.Project.GetById {
    public record GetProjectByIdQuery(
        Guid Id
    ) : IRequest<ProjectDto?>;
}
