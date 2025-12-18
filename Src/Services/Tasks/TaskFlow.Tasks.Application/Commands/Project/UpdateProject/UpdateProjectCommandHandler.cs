using MediatR;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Commands.Project.UpdateProject {
    public class UpdateProjectCommandHandler(IProjectRepository repository) : IRequestHandler<UpdateProjectCommand, bool> {
        private readonly IProjectRepository _repository = repository;
        
        public async Task<bool> Handle(UpdateProjectCommand command, CancellationToken cancellationToken) {
            var project = await _repository.GetByIdAsync(command.Id);
            
            if (project is null) { 
                return false;    
            }

            project.Name = command.Name;
            project.Description = command.Description;
            project.IsActive = command.IsActive;

            var result = await _repository.UpdateAsync(project);

            return result;
        }
    }
}
