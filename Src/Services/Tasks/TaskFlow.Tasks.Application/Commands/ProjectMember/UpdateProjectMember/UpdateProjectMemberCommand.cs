using MediatR;
using TaskFlow.Tasks.Domain.Enums;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Commands.ProjectMember.UpdateProjectMember {
    public record UpdateProjectMemberCommand(
        Guid Id,
        ProjectRole Role
    ) : IRequest<RequestResult<Unit>>;
}
