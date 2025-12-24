using AutoMapper;
using MediatR;
using TaskFlow.Tasks.Application.DTOs;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Queries.Comment.GetById {
    public class GetCommentByIdQueryHandler(IMapper mapper, ICommentRepository repository) : IRequestHandler<GetCommentByIdQuery, RequestResult<CommentDto>> {
        private readonly IMapper _mapper = mapper;
        private readonly ICommentRepository _repository = repository;

        public async Task<RequestResult<CommentDto>> Handle(GetCommentByIdQuery query, CancellationToken cancellationToken) {
            var comment = await _repository.GetByIdAsync(query.Id);

            if (comment is null) {
                return RequestResult<CommentDto>.NotFound("Comment", query.Id);
            }

            return RequestResult<CommentDto>.Success(_mapper.Map<CommentDto>(comment));
        }
    }
}
