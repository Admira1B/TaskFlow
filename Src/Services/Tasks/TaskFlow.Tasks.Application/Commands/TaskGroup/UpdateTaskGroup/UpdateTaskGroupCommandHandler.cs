using MediatR;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Commands.TaskGroup.UpdateTaskGroup {
    public class UpdateTaskGroupCommandHandler(ITaskGroupRepository repository) : IRequestHandler<UpdateTaskGroupCommand, RequestResult<Unit>> {
        private readonly ITaskGroupRepository _repository = repository;

        public Task<RequestResult<Unit>> Handle(UpdateTaskGroupCommand request, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }
    }
}
