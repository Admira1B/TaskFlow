using MediatR;
using TaskFlow.Shared.Core.Results;

namespace TaskFlow.Tasks.Application.Commands.Comment.DeleteComment {
    public record DeleteCommentCommand(
        Guid Id
    ) : IRequest<RequestResult<Unit>>;
}
