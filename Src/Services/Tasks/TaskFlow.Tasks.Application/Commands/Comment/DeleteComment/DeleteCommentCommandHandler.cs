using MediatR;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Commands.Comment.DeleteComment {
    public class DeleteCommentCommandHandler(ICommentRepository repository) : IRequestHandler<DeleteCommentCommand, RequestResult<Unit>> {
        private readonly ICommentRepository _repository = repository;
        
        public async Task<RequestResult<Unit>> Handle(DeleteCommentCommand command, CancellationToken cancellationToken) {
            var comment = await _repository.GetByIdAsync(command.Id);

            if (comment is null) {
                return RequestResult<Unit>.NotFound("Comment", command.Id);
            }

            try {
                await _repository.DeleteAsync(command.Id);
            } catch (Exception) {
                return RequestResult<Unit>.Failure("Failed to delete comment.");
            }

            return RequestResult<Unit>.Success();
        }
    }
}
