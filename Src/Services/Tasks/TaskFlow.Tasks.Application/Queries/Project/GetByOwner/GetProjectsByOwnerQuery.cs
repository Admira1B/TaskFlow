using MediatR;
using TaskFlow.Shared.Core.Results;
using TaskFlow.Tasks.Contracts.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Queries.Project.GetByOwner {
    public record GetProjectsByOwnerQuery(
        Guid UserId
    ) : IRequest<RequestResult<List<ProjectDto>>>;
}
