using MediatR;
using TaskFlow.Shared.Core.Interfaces;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Commands.TaskGroup.UpdateTaskGroup {
    public class UpdateTaskGroupCommandHandler(ILogger logger, ITaskGroupRepository repository) : IRequestHandler<UpdateTaskGroupCommand, RequestResult<Unit>> {
        private readonly ILogger _logger = logger;
        private readonly ITaskGroupRepository _repository = repository;

        public async Task<RequestResult<Unit>> Handle(UpdateTaskGroupCommand command, CancellationToken cancellationToken) {
            _logger.Debug("Task group update attempt. GroupId: {GroupId}, Name: {Name}",
                command.Id.ToString(),
                command.Name
            );

            var group = await _repository.GetByIdAsync(command.Id);

            if (group is null) {
                _logger.Debug("Failed to update task group. Task group {GroupId} not found", command.Id.ToString());
                return RequestResult<Unit>.NotFound("Task Group", command.Id);
            }

            group.Name = command.Name;

            try {
                await _repository.UpdateAsync(group);
            } catch (Exception ex) {
                _logger.Debug("Failed to update task group. GroupId: {GroupId}, Exception: {Message}",
                    command.Id.ToString(),
                    ex.Message
                );

                return RequestResult<Unit>.Failure("Failed to update task group.");
            }

            _logger.Debug("Task group successfully updated. GroupId: {GroupId}, Name: {Name}",
                command.Id.ToString(),
                command.Name
            );

            return RequestResult<Unit>.Success();
        }
    }
}