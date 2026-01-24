using MediatR;
using TaskFlow.Shared.Core.Interfaces;
using TaskFlow.Tasks.Domain.Contracts;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Commands.TaskItem.DeleteTaskItem {
    public class DeleteTaskItemCommandHandler(ILogger logger, ITaskItemRepository repository) : IRequestHandler<DeleteTaskItemCommand, RequestResult<Unit>> {
        private readonly ILogger _logger = logger;
        private readonly ITaskItemRepository _repository = repository;


        public async Task<RequestResult<Unit>> Handle(DeleteTaskItemCommand command, CancellationToken cancellationToken) {
            _logger.Debug("Task item deletion attempt. TaskId: {TaskId}", command.Id.ToString());

            var task = await _repository.GetByIdAsync(command.Id);

            if (task is null) {
                _logger.Debug("Failed to delete task item. Task item {TaskId} not found", command.Id.ToString());
                return RequestResult<Unit>.NotFound("Task", command.Id);
            }

            try {
                await _repository.DeleteAsync(command.Id);
            } catch (Exception ex) {
                _logger.Debug("Failed to delete task item. TaskId: {TaskId}, Exception: {Message}",
                    command.Id.ToString(),
                    ex.Message
                );
                return RequestResult<Unit>.Failure("Failed to delete task.");
            }

            _logger.Debug("Task item successfully deleted. TaskId: {TaskId}", command.Id.ToString());

            return RequestResult<Unit>.Success();
        }
    }
}