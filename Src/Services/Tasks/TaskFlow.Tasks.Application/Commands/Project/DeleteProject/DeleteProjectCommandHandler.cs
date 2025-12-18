using MediatR;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Commands.Project.DeleteProject {
    public class DeleteProjectCommandHandler(IProjectRepository repository) : IRequestHandler<DeleteProjectCommand, bool> {
        private readonly IProjectRepository _repository = repository;
        public async Task<bool> Handle(DeleteProjectCommand command, CancellationToken cancellationToken) {
            var result = await _repository.DeleteAsync(command.Id);
            
            return result;
        }
    }
}
