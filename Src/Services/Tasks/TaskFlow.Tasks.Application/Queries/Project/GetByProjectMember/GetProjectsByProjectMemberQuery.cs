using MediatR;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Contracts.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Queries.Project.GetByProjectMember {
    public record GetProjectsByProjectMemberQuery(
        Guid UserId
    ) : IRequest<RequestResult<List<ProjectDto>>>;
}
