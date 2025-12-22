using MediatR;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Enums;

namespace TaskFlow.Tasks.Application.Commands.ProjectMember.CreateProjectMember {
    public record CreateProjectMemberCommand(
        Guid ProjectId,
        Guid UserId,
        ProjectRole Role
    ) : IRequest<RequestResult<Unit>>;
}
