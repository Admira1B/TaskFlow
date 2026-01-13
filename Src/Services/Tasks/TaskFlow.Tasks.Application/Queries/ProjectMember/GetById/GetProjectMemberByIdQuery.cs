using MediatR;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Application.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Queries.ProjectMember.GetById {
    public record GetProjectMemberByIdQuery(
        Guid Id
    ) : IRequest<RequestResult<ProjectMemberDto>>;
}
