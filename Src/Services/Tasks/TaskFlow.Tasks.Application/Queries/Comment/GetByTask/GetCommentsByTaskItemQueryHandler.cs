using MediatR;
using AutoMapper;
using TaskFlow.Tasks.Domain.Contracts;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Contracts.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Queries.Comment.GetByTask {
    public class GetCommentsByTaskItemQueryHandler(IMapper mapper, ICommentRepository repository, ITaskItemRepository taskRepository) : IRequestHandler<GetCommentsByTaskItemQuery, RequestResult<List<CommentDto>>> {
        private readonly IMapper _mapper = mapper;
        private readonly ICommentRepository _repository = repository;
        private readonly ITaskItemRepository _taskRepository = taskRepository;

        public async Task<RequestResult<List<CommentDto>>> Handle(GetCommentsByTaskItemQuery query, CancellationToken cancellationToken) {
            var task = await _taskRepository.GetByIdAsync(query.TaskItemId);

            if (task is null) {
                return RequestResult<List<CommentDto>>.NotFound("Task", query.TaskItemId);
            }
            
            var comments = await _repository.GetByTaskIdAsync(query.TaskItemId);

            return RequestResult<List<CommentDto>>.Success(_mapper.Map<List<CommentDto>>(comments));
        }
    }
}
