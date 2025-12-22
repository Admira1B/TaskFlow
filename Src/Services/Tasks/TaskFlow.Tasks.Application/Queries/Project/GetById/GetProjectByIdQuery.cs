using MediatR;
using TaskFlow.Tasks.Application.DTOs;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Queries.Project.GetById {
    public record GetProjectByIdQuery(
        Guid Id
    ) : IRequest<RequestResult<ProjectDto>>;
}
