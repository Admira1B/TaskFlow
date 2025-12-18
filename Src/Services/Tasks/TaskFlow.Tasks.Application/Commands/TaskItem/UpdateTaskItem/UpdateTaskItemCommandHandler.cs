using MediatR;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Commands.TaskItem.UpdateTaskItem {
    public class UpdateTaskItemCommandHandler(ITaskItemRepository repository) : IRequestHandler<UpdateTaskItemCommand, bool> {
        private readonly ITaskItemRepository _repository = repository;

        public async Task<bool> Handle(UpdateTaskItemCommand command, CancellationToken cancellationToken) {
            var task = await _repository.GetByIdAsync(command.Id);

            if (task is null) { 
                return false;
            }

            task.Title = command.Title;
            task.Description = command.Description;
            task.AssignedId = command.AssignedId;
            task.Priority = command.Priority;

            return await _repository.UpdateAsync(task);
        }
    }
}
