using MediatR;
using AutoMapper;
using TaskFlow.Shared.Core.Results;
using TaskFlow.Shared.Core.Interfaces;
using TaskFlow.Tasks.Domain.Contracts;
using TaskFlow.Tasks.Contracts.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Commands.Comment.CreateComment {
    public class CreateCommentCommandHandler(ILogger logger, IMapper mapper, ICommentRepository repository) : IRequestHandler<CreateCommentCommand, RequestResult<CommentDto>> {
        private readonly ILogger _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly ICommentRepository _repository = repository;

        public async Task<RequestResult<CommentDto>> Handle(CreateCommentCommand command, CancellationToken cancellationToken = default) {
            _logger.Debug("Comment creating attempt. TaskId: {TaskId}, AuthorId: {AuthorId}, ContentLength: {ContentLength}",
                command.TaskId, 
                command.AuthorId, 
                command.Content?.Length.ToString() ?? "0"
            );

            var comment = new Domain.Entities.Comment() {
                TaskId = command.TaskId,
                Content = command.Content ?? string.Empty,
                AuthorId = command.AuthorId
            };

            try {
                await _repository.AddAsync(comment, cancellationToken);
            } catch (Exception ex) {
                _logger.Debug("Failed to create comment. TaskId: {TaskId}, AuthorId: {AuthorId}, Exception {Message}", command.TaskId, command.AuthorId, ex.Message);
                return RequestResult<CommentDto>.Failure("Failed to create comment.");
            }

            _logger.Debug("Comment successfully created. CommentId: {CommentId}, TaskId: {TaskId}, AuthorId: {AuthorId}",
                comment.Id,
                comment.TaskId,
                comment.AuthorId
            );

            return RequestResult<CommentDto>.Success(_mapper.Map<CommentDto>(comment));
        }
    }
}
