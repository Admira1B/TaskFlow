using MediatR;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Commands.ProjectMember.DeleteProjectMember {
    public record DeleteProjectMemberCommand(
        Guid MemberId
    ) : IRequest<RequestResult<Unit>>;
}
