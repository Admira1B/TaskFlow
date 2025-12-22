using MediatR;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Commands.TaskItem.DeleteTaskItem {
    public class DeleteTaskItemCommandHandler(ITaskItemRepository repository) : IRequestHandler<DeleteTaskItemCommand, RequestResult<Unit>> {
        private readonly ITaskItemRepository _repository = repository;


        public async Task<RequestResult<Unit>> Handle(DeleteTaskItemCommand command, CancellationToken cancellationToken) {
            var task = await _repository.GetByIdAsync(command.Id);

            if (task is null) {
                return RequestResult<Unit>.NotFound("Task");
            }

            try {
                await _repository.DeleteAsync(command.Id);
            } catch (Exception) {
                return RequestResult<Unit>.Failure("Failed to delete task.");
            }

            return RequestResult<Unit>.Success();
        }
    }
}
