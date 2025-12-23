using MediatR;
using TaskFlow.Tasks.Application.DTOs;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Queries.ProjectMember.GetById {
    public record GetProjectMemberByIdQuery(
        Guid Id
    ) : IRequest<RequestResult<ProjectMemberDto>>;
}
