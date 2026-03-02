using MediatR;
using TaskFlow.Shared.Core.Results;

namespace TaskFlow.Tasks.Application.Commands.ProjectMember.DeleteProjectMember {
    public record DeleteProjectMemberCommand(
        Guid Id
    ) : IRequest<RequestResult<Unit>>;
}
