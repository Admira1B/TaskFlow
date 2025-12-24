using AutoMapper;
using MediatR;
using TaskFlow.Tasks.Application.DTOs;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Commands.Comment.CreateComment {
    public class CreateCommentCommandHandler(IMapper mapper, ICommentRepository repository) : IRequestHandler<CreateCommentCommand, RequestResult<CommentDto>> {
        private readonly IMapper _mapper = mapper;
        private readonly ICommentRepository _repository = repository;
        
        public async Task<RequestResult<CommentDto>> Handle(CreateCommentCommand command, CancellationToken cancellationToken) {
            var comment = new Domain.Entities.Comment() { 
                TaskId = command.TaskId,
                Content = command.Content,
                AuthorId = command.AuthorId
            };

            try {
                await _repository.AddAsync(comment);
            } catch (Exception) {
                return RequestResult<CommentDto>.Failure("Failed to create comment.");
            }

            return RequestResult<CommentDto>.Success(_mapper.Map<CommentDto>(comment));
        }
    }
}
