using MediatR;
using TaskFlow.Tasks.Application.DTOs;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Queries.ProjectMember.GetByProject {
    public record GetProjectsMembersByProjectQuery(
        Guid ProjectId    
    ) : IRequest<RequestResult<List<ProjectMemberDto>>>;
}
