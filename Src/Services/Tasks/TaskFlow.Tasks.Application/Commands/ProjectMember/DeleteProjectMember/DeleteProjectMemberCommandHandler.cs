using MediatR;
using TaskFlow.Shared.Core.Interfaces;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Commands.ProjectMember.DeleteProjectMember {
    public class DeleteProjectMemberCommandHandler(ILogger logger, IProjectMemberRepository repository) : IRequestHandler<DeleteProjectMemberCommand, RequestResult<Unit>> {
        private readonly ILogger _logger = logger;
        private readonly IProjectMemberRepository _repository = repository;

        public async Task<RequestResult<Unit>> Handle(DeleteProjectMemberCommand command, CancellationToken cancellationToken = default) {
            _logger.Debug("Project member deletion attempt. MemberId: {MemberId}", command.Id.ToString());

            var member = await _repository.GetByIdAsync(command.Id, cancellationToken);

            if (member is null) {
                _logger.Debug("Failed to delete project member. Project {MemberId} member not found", command.Id.ToString());
                return RequestResult<Unit>.NotFound("Project Member", command.Id);
            }

            try {
                await _repository.DeleteAsync(command.Id, cancellationToken);
            } catch (Exception ex) {
                _logger.Debug("Failed to delete project member. MemberId: {MemberId}, Exception: {Message}",
                    command.Id.ToString(),
                    ex.Message
                );

                return RequestResult<Unit>.Failure("Failed to delete project member.");
            }

            _logger.Debug("Project member successfully deleted. MemberId: {MemberId}", command.Id.ToString());

            return RequestResult<Unit>.Success();
        }
    }
}