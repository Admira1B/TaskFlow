using MediatR;
using TaskFlow.Shared.Core.Results;
using TaskFlow.Tasks.Contracts.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Queries.ProjectMember.GetById {
    public record GetProjectMemberByIdQuery(
        Guid Id
    ) : IRequest<RequestResult<ProjectMemberDto>>;
}
