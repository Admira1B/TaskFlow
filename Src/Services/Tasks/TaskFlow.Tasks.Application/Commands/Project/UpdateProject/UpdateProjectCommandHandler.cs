using MediatR;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Commands.Project.UpdateProject {
    public class UpdateProjectCommandHandler(IProjectRepository repository) : IRequestHandler<UpdateProjectCommand, RequestResult<Unit>> {
        private readonly IProjectRepository _repository = repository;
        
        public async Task<RequestResult<Unit>> Handle(UpdateProjectCommand command, CancellationToken cancellationToken) {
            var project = await _repository.GetByIdAsync(command.Id);

            if (project is null) {
                return RequestResult<Unit>.NotFound(nameof(Domain.Entities.Project));
            }

            project.Name = command.Name;
            project.Description = command.Description;
            project.IsActive = command.IsActive;

            try {
                await _repository.UpdateAsync(project);
            } catch (Exception) {
                return RequestResult<Unit>.Failure("Failed to update project.");
            }

            return RequestResult<Unit>.Success();
        }
    }
}
