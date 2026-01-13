using MediatR;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Application.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Queries.Project.GetByOwner {
    public record GetProjectsByOwnerQuery(
        Guid UserId
    ) : IRequest<RequestResult<List<ProjectDto>>>;
}
