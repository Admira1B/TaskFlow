using MediatR;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Application.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Queries.Project.GetById {
    public record GetProjectByIdQuery(
        Guid Id
    ) : IRequest<RequestResult<ProjectDto>>;
}
