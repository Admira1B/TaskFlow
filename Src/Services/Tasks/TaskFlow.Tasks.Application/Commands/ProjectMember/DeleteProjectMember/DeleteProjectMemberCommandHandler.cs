using MediatR;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Commands.ProjectMember.DeleteProjectMember {
    public class DeleteProjectMemberCommandHandler : IRequestHandler<DeleteProjectMemberCommand, RequestResult<Unit>> {
        public Task<RequestResult<Unit>> Handle(DeleteProjectMemberCommand command, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }
    }
}
