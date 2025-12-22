using MediatR;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Commands.TaskGroup.CreateTaskGroup {
    public class CreateTaskGroupCommandHandler : IRequestHandler<CreateTaskGroupCommand, RequestResult<Unit>> {
        public Task<RequestResult<Unit>> Handle(CreateTaskGroupCommand command, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }
    }
}
