using MediatR;
using TaskFlow.Tasks.Application.DTOs.Responses;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Queries.ProjectMember.GetById {
    public record GetProjectMemberByIdQuery(
        Guid Id
    ) : IRequest<RequestResult<ProjectMemberDto>>;
}
