using MediatR;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Application.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Queries.ProjectMember.GetByUser {
    public record GetProjectsMembersByUserQuery(
        Guid UserId    
    ) : IRequest<RequestResult<List<ProjectMemberDto>>>;
}
