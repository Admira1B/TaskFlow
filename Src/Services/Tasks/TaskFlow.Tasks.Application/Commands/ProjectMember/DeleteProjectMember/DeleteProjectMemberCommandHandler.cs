using MediatR;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Commands.ProjectMember.DeleteProjectMember {
    public class DeleteProjectMemberCommandHandler(IProjectMemberRepository repository) : IRequestHandler<DeleteProjectMemberCommand, RequestResult<Unit>> {
        private readonly IProjectMemberRepository _repository = repository;
        
        public async Task<RequestResult<Unit>> Handle(DeleteProjectMemberCommand command, CancellationToken cancellationToken) {
            var member = await _repository.GetByIdAsync(command.Id);

            if (member is null) {
                return RequestResult<Unit>.NotFound("Project Member", command.Id);
            }

            try {
                await _repository.DeleteAsync(command.Id);
            } catch (Exception) {
                return RequestResult<Unit>.Failure("Failed to delete project member.");
            }

            return RequestResult<Unit>.Success();
        }
    }
}
