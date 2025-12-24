using MediatR;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Commands.ProjectMember.UpdateProjectMember {
    public class UpdateProjectMemberCommandHandler(IProjectMemberRepository repository) : IRequestHandler<UpdateProjectMemberCommand, RequestResult<Unit>> {
        private readonly IProjectMemberRepository _repository = repository;

        public async Task<RequestResult<Unit>> Handle(UpdateProjectMemberCommand command, CancellationToken cancellationToken) {
            var member = await _repository.GetByIdAsync(command.Id);

            if (member is null) {
                return RequestResult<Unit>.NotFound("Project Member", command.Id);
            }

            member.Role = command.Role;

            try {
                await _repository.UpdateAsync(member);
            } catch (Exception) {
                return RequestResult<Unit>.Failure("Failed to update project member.");
            }

            return RequestResult<Unit>.Success();
        }
    }
}
