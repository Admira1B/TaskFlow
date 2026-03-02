using MediatR;
using AutoMapper;
using TaskFlow.Shared.Core.Results;
using TaskFlow.Tasks.Domain.Contracts;
using TaskFlow.Tasks.Contracts.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Queries.Comment.GetById {
    public class GetCommentByIdQueryHandler(IMapper mapper, ICommentRepository repository) : IRequestHandler<GetCommentByIdQuery, RequestResult<CommentDto>> {
        private readonly IMapper _mapper = mapper;
        private readonly ICommentRepository _repository = repository;

        public async Task<RequestResult<CommentDto>> Handle(GetCommentByIdQuery query, CancellationToken cancellationToken = default) {
            var comment = await _repository.GetByIdAsync(query.Id, cancellationToken);

            if (comment is null) {
                return RequestResult<CommentDto>.NotFound("Comment", query.Id);
            }

            return RequestResult<CommentDto>.Success(_mapper.Map<CommentDto>(comment));
        }
    }
}
