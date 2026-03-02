using MediatR;
using TaskFlow.Shared.Core.Results;
using TaskFlow.Tasks.Contracts.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Queries.Project.GetById {
    public record GetProjectByIdQuery(
        Guid Id
    ) : IRequest<RequestResult<ProjectDto>>;
}
