using MediatR;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Commands.TaskItem.UpdateTaskItem {
    public class UpdateTaskItemCommandHandler(ITaskItemRepository repository) : IRequestHandler<UpdateTaskItemCommand, RequestResult<Unit>> {
        private readonly ITaskItemRepository _repository = repository;

        public async Task<RequestResult<Unit>> Handle(UpdateTaskItemCommand command, CancellationToken cancellationToken) {
            var task = await _repository.GetByIdAsync(command.Id);

            if (task is null) {
                return RequestResult<Unit>.NotFound("Task", command.Id);
            }

            task.Title = command.Title;
            task.Description = command.Description;
            task.AssignedId = command.AssignedId;
            task.Priority = command.Priority;
            task.GroupId = command.GroupId;

            try {
                await _repository.UpdateAsync(task);
            } catch (Exception) {
                return RequestResult<Unit>.Failure("Failed to update task.");
            }

            return RequestResult<Unit>.Success();
        }
    }
}
