using MediatR;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Commands.Comment.UpdateComment {
    public class UpdateCommentCommandHandler(ICommentRepository repository) : IRequestHandler<UpdateCommentCommand, RequestResult<Unit>> {
        private readonly ICommentRepository _repository = repository;
        
        public Task<RequestResult<Unit>> Handle(UpdateCommentCommand request, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }
    }
}
