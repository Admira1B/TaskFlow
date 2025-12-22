using MediatR;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Commands.TaskGroup.DeleteTaskGroup {
    public class DeleteTaskGroupCommandHandler : IRequestHandler<DeleteTaskGroupCommand, RequestResult<Unit>> {
        public Task<RequestResult<Unit>> Handle(DeleteTaskGroupCommand request, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }
    }
}
