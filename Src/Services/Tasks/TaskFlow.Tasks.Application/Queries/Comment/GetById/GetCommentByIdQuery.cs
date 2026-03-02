using MediatR;
using TaskFlow.Shared.Core.Results;
using TaskFlow.Tasks.Contracts.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Queries.Comment.GetById {
    public record GetCommentByIdQuery (
        Guid Id
    ) : IRequest<RequestResult<CommentDto>>;
}
