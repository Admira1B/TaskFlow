using MediatR;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Commands.Comment.CreateComment {
    public class CreateCommentCommandHandler(ICommentRepository repository) : IRequestHandler<CreateCommentCommand, RequestResult<Unit>> {
        private readonly ICommentRepository _repository = repository;
        
        public async Task<RequestResult<Unit>> Handle(CreateCommentCommand command, CancellationToken cancellationToken) {
            var comment = new Domain.Entities.Comment() { 
                TaskId = command.TaskId,
                Content = command.Content,
                AuthorId = command.AuthorId
            };

            return RequestResult<Unit>.Success();
        }
    }
}
