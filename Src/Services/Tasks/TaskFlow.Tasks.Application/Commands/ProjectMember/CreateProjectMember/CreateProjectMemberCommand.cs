using MediatR;
using TaskFlow.Tasks.Domain.Enums;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Application.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Commands.ProjectMember.CreateProjectMember {
    public record CreateProjectMemberCommand(
        Guid ProjectId,
        Guid UserId,
        ProjectRole Role
    ) : IRequest<RequestResult<ProjectMemberDto>>;
}
