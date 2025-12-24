using MediatR;
using TaskFlow.Tasks.Application.DTOs;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Queries.Project.GetByProjectMember {
    public record GetProjectsByProjectMemberQuery(
        Guid UserId
    ) : IRequest<RequestResult<List<ProjectDto>>>;
}
