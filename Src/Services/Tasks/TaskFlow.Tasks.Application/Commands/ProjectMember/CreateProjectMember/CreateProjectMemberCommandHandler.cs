using MediatR;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Commands.ProjectMember.CreateProjectMember {
    public class CreateProjectMemberCommandHandler : IRequestHandler<CreateProjectMemberCommand, RequestResult<Unit>> {
        public Task<RequestResult<Unit>> Handle(CreateProjectMemberCommand command, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }
    }
}
