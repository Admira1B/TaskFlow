using MediatR;
using TaskFlow.Tasks.Application.DTOs.Responses;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Queries.Project.GetByOwner {
    public record GetProjectsByOwnerQuery(
        Guid UserId
    ) : IRequest<RequestResult<List<ProjectDto>>>;
}
