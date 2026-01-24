using MediatR;
using TaskFlow.Shared.Core.Interfaces;
using TaskFlow.Tasks.Domain.Contracts;
using TaskFlow.Tasks.Application.Results;

namespace TaskFlow.Tasks.Application.Commands.Comment.DeleteComment {
    public class DeleteCommentCommandHandler(ILogger logger, ICommentRepository repository) : IRequestHandler<DeleteCommentCommand, RequestResult<Unit>> {
        private readonly ILogger _logger = logger;
        private readonly ICommentRepository _repository = repository;
        
        public async Task<RequestResult<Unit>> Handle(DeleteCommentCommand command, CancellationToken cancellationToken) {
            _logger.Debug("Comment deletion attempt. CommentId: {CommentId}", command.Id.ToString());

            var comment = await _repository.GetByIdAsync(command.Id);

            if (comment is null) {
                _logger.Debug("Failed to delete comment. Comment {CommentId} not found", command.Id.ToString());
                return RequestResult<Unit>.NotFound("Comment", command.Id);
            }

            try {
                await _repository.DeleteAsync(command.Id);
            } catch (Exception ex) {
                _logger.Debug("Failed to delete comment. CommentId: {CommentId}, Exception: {Message}", command.Id.ToString(), ex.Message);
                return RequestResult<Unit>.Failure("Failed to delete comment.");
            }

            _logger.Debug("Comment successfully deleted. CommentId: {CommentId}", command.Id.ToString());

            return RequestResult<Unit>.Success();
        }
    }
}
