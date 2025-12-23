using AutoMapper;
using MediatR;
using TaskFlow.Tasks.Application.DTOs;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Queries.TaskItem.GetById {
    public class GetTaskItemByIdQueryHandler(IMapper mapper, ITaskItemRepository repository) : IRequestHandler<GetTaskItemByIdQuery, RequestResult<TaskItemDto>> {
        private readonly IMapper _mapper = mapper;
        private readonly ITaskItemRepository _repository = repository;

        public async Task<RequestResult<TaskItemDto>> Handle(GetTaskItemByIdQuery query, CancellationToken cancellationToken) {
            var task = await _repository.GetByIdAsync(query.Id);

            if (task is null) {
                return RequestResult<TaskItemDto>.NotFound("Task", query.Id);
            }

            return RequestResult<TaskItemDto>.Success(_mapper.Map<TaskItemDto>(task));
        }
    }
}
