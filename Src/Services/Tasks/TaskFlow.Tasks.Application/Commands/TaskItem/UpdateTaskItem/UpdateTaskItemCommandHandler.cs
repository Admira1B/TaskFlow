using MediatR;
using TaskFlow.Shared.Core.Interfaces;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Commands.TaskItem.UpdateTaskItem {
    public class UpdateTaskItemCommandHandler(ILogger logger, ITaskItemRepository repository) : IRequestHandler<UpdateTaskItemCommand, RequestResult<Unit>> {
        private readonly ILogger _logger = logger;
        private readonly ITaskItemRepository _repository = repository;

        public async Task<RequestResult<Unit>> Handle(UpdateTaskItemCommand command, CancellationToken cancellationToken = default) {
            _logger.Debug("Task item update attempt. TaskId: {TaskId}, Title: {Title}, GroupId: {GroupId}, AssignedId: {AssignedId}, Priority: {Priority}, DescriptionLength: {DescriptionLength}",
                command.Id.ToString(),
                command.Title,
                command.GroupId.ToString(),
                command.AssignedId?.ToString() ?? "null",
                command.Priority.ToString(),
                command.Description?.Length.ToString() ?? "0"
            );

            var task = await _repository.GetByIdAsync(command.Id, cancellationToken);

            if (task is null) {
                _logger.Debug("Failed to update task item. Task item {TaskId} not found", command.Id.ToString());
                return RequestResult<Unit>.NotFound("Task", command.Id);
            }

            task.Title = command.Title;
            task.Description = command.Description;
            task.AssignedId = command.AssignedId;
            task.Priority = command.Priority;
            task.GroupId = command.GroupId;

            try {
                await _repository.UpdateAsync(task, cancellationToken);
            } catch (Exception ex) {
                _logger.Debug("Failed to update task item. TaskId: {TaskId}, Exception: {Message}",
                    command.Id.ToString(),
                    ex.Message
                );
                return RequestResult<Unit>.Failure("Failed to update task.");
            }

            _logger.Info("Task item successfully updated. TaskId: {TaskId}", command.Id.ToString());

            return RequestResult<Unit>.Success();
        }
    }
}
