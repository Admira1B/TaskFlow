using MediatR;
using TaskFlow.Shared.Core.Results;
using TaskFlow.Tasks.Domain.Enums;
using TaskFlow.Tasks.Contracts.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Commands.ProjectMember.CreateProjectMember {
    public record CreateProjectMemberCommand(
        Guid ProjectId,
        Guid UserId,
        ProjectRole Role
    ) : IRequest<RequestResult<ProjectMemberDto>>;
}
