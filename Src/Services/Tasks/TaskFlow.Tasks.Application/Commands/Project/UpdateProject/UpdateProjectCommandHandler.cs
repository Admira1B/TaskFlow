using MediatR;
using TaskFlow.Shared.Core.Interfaces;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Commands.Project.UpdateProject {
    public class UpdateProjectCommandHandler(ILogger logger, IProjectRepository repository) : IRequestHandler<UpdateProjectCommand, RequestResult<Unit>> {
        private readonly ILogger _logger = logger;
        private readonly IProjectRepository _repository = repository;
        
        public async Task<RequestResult<Unit>> Handle(UpdateProjectCommand command, CancellationToken cancellationToken) {
            _logger.Debug("Project update attempt. ProjectId: {ProjectId}, Name: {Name}, IsActive: {IsActive}, DescriptionLength: {DescriptionLength}",
                command.Id.ToString(),
                command.Name,
                command.IsActive.ToString(),
                command.Description?.Length.ToString() ?? "0"
            );

            var project = await _repository.GetByIdAsync(command.Id);

            if (project is null) {
                _logger.Debug("Failed to update project. Project {ProjectId} not found", command.Id.ToString());
                return RequestResult<Unit>.NotFound("Project", command.Id);
            }

            project.Name = command.Name;
            project.Description = command.Description;
            project.IsActive = command.IsActive;

            try {
                await _repository.UpdateAsync(project);
            } catch (Exception ex) {
                _logger.Debug("Failed to update project. ProjectId: {ProjectId}, Exception: {Message}",
                    command.Id.ToString(),
                    ex.Message
                );

                return RequestResult<Unit>.Failure("Failed to update project.");
            }

            _logger.Debug("Project successfully updated. ProjectId: {ProjectId}", command.Id.ToString());

            return RequestResult<Unit>.Success();
        }
    }
}
