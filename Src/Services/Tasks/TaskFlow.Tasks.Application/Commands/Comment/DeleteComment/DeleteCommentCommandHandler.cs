using MediatR;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Commands.Comment.DeleteComment {
    public class DeleteCommentCommandHandler(ICommentRepository repository) : IRequestHandler<DeleteCommentCommand, RequestResult<Unit>> {
        private readonly ICommentRepository _repository = repository;
        
        public Task<RequestResult<Unit>> Handle(DeleteCommentCommand command, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }
    }
}
