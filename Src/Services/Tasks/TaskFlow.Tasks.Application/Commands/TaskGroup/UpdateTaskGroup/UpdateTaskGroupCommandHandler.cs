using MediatR;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Commands.TaskGroup.UpdateTaskGroup {
    public class UpdateTaskGroupCommandHandler(ITaskGroupRepository repository) : IRequestHandler<UpdateTaskGroupCommand, RequestResult<Unit>> {
        private readonly ITaskGroupRepository _repository = repository;

        public async Task<RequestResult<Unit>> Handle(UpdateTaskGroupCommand command, CancellationToken cancellationToken) {
            var group = await _repository.GetByIdAsync(command.Id);

            if (group is null) {
                return RequestResult<Unit>.NotFound("Task Group", command.Id);
            }

            group.Name = command.Name;

            try {
                await _repository.UpdateAsync(group);
            } catch (Exception) {
                return RequestResult<Unit>.Failure("Failed to update task group.");
            }

            return RequestResult<Unit>.Success();
        }
    }
}
