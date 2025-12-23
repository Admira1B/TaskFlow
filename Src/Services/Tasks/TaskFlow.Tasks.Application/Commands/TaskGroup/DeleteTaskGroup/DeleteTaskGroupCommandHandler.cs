using MediatR;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Commands.TaskGroup.DeleteTaskGroup {
    public class DeleteTaskGroupCommandHandler(ITaskGroupRepository repository) : IRequestHandler<DeleteTaskGroupCommand, RequestResult<Unit>> {
        private readonly ITaskGroupRepository _repository = repository;

        public async Task<RequestResult<Unit>> Handle(DeleteTaskGroupCommand command, CancellationToken cancellationToken) {
            var group = await _repository.GetByIdAsync(command.Id);

            if (group is null) {
                return RequestResult<Unit>.NotFound("Task Group", command.Id);
            }

            try {
                await _repository.DeleteAsync(command.Id);
            } catch (Exception) {
                return RequestResult<Unit>.Failure("Failed to delete task group.");
            }

            return RequestResult<Unit>.Success();
        }
    }
}
