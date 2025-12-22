using MediatR;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Commands.TaskGroup.DeleteProject {
    public class DeleteProjectCommandHandler(IProjectRepository repository) : IRequestHandler<DeleteProjectCommand, RequestResult<Unit>> {
        private readonly IProjectRepository _repository = repository;

        public async Task<RequestResult<Unit>> Handle(DeleteProjectCommand command, CancellationToken cancellationToken) {
            var project = await _repository.GetByIdAsync(command.Id);

            if (project is null) {
                return RequestResult<Unit>.NotFound(nameof(Domain.Entities.Project));
            }

            try {
                await _repository.DeleteAsync(command.Id);
            } catch (Exception) {
                return RequestResult<Unit>.Failure("Failed to delete project.");
            }

            return RequestResult<Unit>.Success();
        }
    }
}
