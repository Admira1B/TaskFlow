using MediatR;
using TaskFlow.Shared.Core.Interfaces;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Commands.ProjectMember.UpdateProjectMember {
    public class UpdateProjectMemberCommandHandler(ILogger logger, IProjectMemberRepository repository) : IRequestHandler<UpdateProjectMemberCommand, RequestResult<Unit>> {
        private readonly ILogger _logger = logger;
        private readonly IProjectMemberRepository _repository = repository;

        public async Task<RequestResult<Unit>> Handle(UpdateProjectMemberCommand command, CancellationToken cancellationToken) {
            _logger.Debug("Project member update attempt. MemberId: {MemberId}, Role: {Role}",
                command.Id.ToString(),
                command.Role.ToString()
            );

            var member = await _repository.GetByIdAsync(command.Id);

            if (member is null) {
                _logger.Debug("Failed to update project member. Project {MemberId} member not found", command.Id.ToString());
                return RequestResult<Unit>.NotFound("Project Member", command.Id);
            }

            member.Role = command.Role;

            try {
                await _repository.UpdateAsync(member);
            } catch (Exception ex) {
                _logger.Debug("Failed to update project member. MemberId: {MemberId}, Exception: {Message}",
                    command.Id.ToString(),
                    ex.Message
                );

                return RequestResult<Unit>.Failure("Failed to update project member.");
            }

            _logger.Debug("Project member successfully updated. MemberId: {MemberId}, Role: {Role}",
                command.Id.ToString(),
                command.Role.ToString()
            );

            return RequestResult<Unit>.Success();
        }
    }
}