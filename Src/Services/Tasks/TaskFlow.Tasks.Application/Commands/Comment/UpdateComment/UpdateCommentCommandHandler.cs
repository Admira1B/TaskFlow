using MediatR;
using TaskFlow.Shared.Core.Results;
using TaskFlow.Shared.Core.Interfaces;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Commands.Comment.UpdateComment {
    public class UpdateCommentCommandHandler(ILogger logger, ICommentRepository repository) : IRequestHandler<UpdateCommentCommand, RequestResult<Unit>> {
        private readonly ILogger _logger = logger;
        private readonly ICommentRepository _repository = repository;
        
        public async Task<RequestResult<Unit>> Handle(UpdateCommentCommand command, CancellationToken cancellationToken = default) {
            _logger.Debug("Comment update attempt. CommentId: {CommentId}, ContentLength: {ContentLength}",
                command.Id.ToString(),
                command.Content?.Length.ToString() ?? "0"
            );

            var comment = await _repository.GetByIdAsync(command.Id, cancellationToken);

            if (comment is null) {
                _logger.Debug("Failed to update comment. Comment {CommentId} not found", command.Id.ToString());
                return RequestResult<Unit>.NotFound("Comment", command.Id);
            }

            comment.Content = command.Content!;

            try {
                await _repository.UpdateAsync(comment, cancellationToken);
            } catch (Exception ex) {
                _logger.Debug("Failed to update comment. CommentId: {CommentId}, Exception: {Message}", 
                    command.Id.ToString(),
                    ex.Message
                );

                return RequestResult<Unit>.Failure("Failed to update comment.");
            }

            _logger.Debug("Comment successfully updated. CommentId: {CommentId}", command.Id.ToString());

            return RequestResult<Unit>.Success();
        }
    }
}
