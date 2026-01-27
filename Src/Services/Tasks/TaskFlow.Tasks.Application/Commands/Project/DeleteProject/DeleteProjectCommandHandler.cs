using MediatR;
using TaskFlow.Tasks.Domain.Contracts;
using TaskFlow.Shared.Core.Interfaces;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Commands.Project.DeleteProject {
    public class DeleteProjectCommandHandler(ILogger logger, IProjectRepository repository) : IRequestHandler<DeleteProjectCommand, RequestResult<Unit>> {
        private readonly ILogger _logger = logger;
        private readonly IProjectRepository _repository = repository;

        public async Task<RequestResult<Unit>> Handle(DeleteProjectCommand command, CancellationToken cancellationToken) {
            _logger.Debug("Project deletion attempt. ProjectId: {ProjectId}", command.Id.ToString());

            var project = await _repository.GetByIdAsync(command.Id);

            if (project is null) {
                _logger.Debug("Failed to delete project. Project {ProjectId} not found", command.Id.ToString());
                return RequestResult<Unit>.NotFound("Project", command.Id);
            }

            try {
                await _repository.DeleteAsync(command.Id);
            } catch (Exception ex) {
                _logger.Debug("Failed to delete project. ProjectId: {ProjectId}, Exception: {Message}",
                    command.Id.ToString(),
                    ex.Message
                );

                return RequestResult<Unit>.Failure("Failed to delete project.");
            }

            _logger.Debug("Project successfully deleted. ProjectId: {ProjectId}", command.Id.ToString());

            return RequestResult<Unit>.Success();
        }
    }
}
