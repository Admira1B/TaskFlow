using MediatR;
using TaskFlow.Shared.Core.Results;
using TaskFlow.Tasks.Domain.Enums;

namespace TaskFlow.Tasks.Application.Commands.ProjectMember.UpdateProjectMember {
    public record UpdateProjectMemberCommand(
        Guid Id,
        ProjectRole Role
    ) : IRequest<RequestResult<Unit>>;
}
