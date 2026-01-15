using MediatR;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Contracts.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Queries.ProjectMember.GetByProject {
    public record GetProjectsMembersByProjectQuery(
        Guid ProjectId    
    ) : IRequest<RequestResult<List<ProjectMemberDto>>>;
}
