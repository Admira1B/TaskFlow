using MediatR;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Commands.TaskItem.DeleteTaskItem {
    public class DeleteTaskItemCommandHandler(ITaskItemRepository repository) : IRequestHandler<DeleteTaskItemCommand, bool> {
        private readonly ITaskItemRepository _repository = repository;


        public async Task<bool> Handle(DeleteTaskItemCommand command, CancellationToken cancellationToken) {
            var result = await _repository.DeleteAsync(command.Id);

            return result;
        }
    }
}
