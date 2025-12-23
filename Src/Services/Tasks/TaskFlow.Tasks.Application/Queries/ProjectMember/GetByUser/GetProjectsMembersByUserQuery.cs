using MediatR;
using TaskFlow.Tasks.Application.DTOs;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Queries.ProjectMember.GetByUser {
    public record GetProjectsMembersByUserQuery(
        Guid UserId    
    ) : IRequest<RequestResult<List<ProjectMemberDto>>>;
}
