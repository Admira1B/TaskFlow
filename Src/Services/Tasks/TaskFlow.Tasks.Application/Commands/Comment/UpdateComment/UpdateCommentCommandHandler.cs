using MediatR;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Commands.Comment.UpdateComment {
    public class UpdateCommentCommandHandler(ICommentRepository repository) : IRequestHandler<UpdateCommentCommand, RequestResult<Unit>> {
        private readonly ICommentRepository _repository = repository;
        
        public async Task<RequestResult<Unit>> Handle(UpdateCommentCommand command, CancellationToken cancellationToken) {
            var comment = await _repository.GetByIdAsync(command.Id);

            if (comment is null) {
                return RequestResult<Unit>.NotFound("Comment", command.Id);
            }

            comment.Content = command.Content;

            try {
                await _repository.UpdateAsync(comment);
            } catch (Exception) {
                return RequestResult<Unit>.Failure("Failed to update comment.");
            }

            return RequestResult<Unit>.Success();
        }
    }
}
