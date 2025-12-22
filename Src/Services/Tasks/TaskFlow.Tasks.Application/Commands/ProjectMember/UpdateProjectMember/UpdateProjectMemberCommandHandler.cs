using MediatR;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Commands.ProjectMember.UpdateProjectMember {
    public class UpdateProjectMemberCommandHandler(IProjectMemberRepository repository) : IRequestHandler<UpdateProjectMemberCommand, RequestResult<Unit>> {
        private readonly IProjectMemberRepository _repository = repository;

        public async Task<RequestResult<Unit>> Handle(UpdateProjectMemberCommand request, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }
    }
}
