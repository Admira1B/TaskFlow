using MediatR;
using TaskFlow.Shared.Core.Interfaces;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Commands.TaskGroup.DeleteTaskGroup {
    public class DeleteTaskGroupCommandHandler(ILogger logger, ITaskGroupRepository repository) : IRequestHandler<DeleteTaskGroupCommand, RequestResult<Unit>> {
        private readonly ILogger _logger = logger;
        private readonly ITaskGroupRepository _repository = repository;

        public async Task<RequestResult<Unit>> Handle(DeleteTaskGroupCommand command, CancellationToken cancellationToken) {
            _logger.Debug("Task group deletion attempt. GroupId: {GroupId}", command.Id.ToString());

            var group = await _repository.GetByIdAsync(command.Id);

            if (group is null) {
                _logger.Debug("Failed to delete task group. Task group {GroupId} not found", command.Id.ToString());
                return RequestResult<Unit>.NotFound("Task Group", command.Id);
            }

            try {
                await _repository.DeleteAsync(command.Id);
            } catch (Exception ex) {
                _logger.Debug("Failed to delete task group. GroupId: {GroupId}, Exception: {Message}",
                    command.Id.ToString(),
                    ex.Message
                );

                return RequestResult<Unit>.Failure("Failed to delete task group.");
            }

            _logger.Debug("Task group successfully deleted. GroupId: {GroupId}", command.Id.ToString());

            return RequestResult<Unit>.Success();
        }
    }
}