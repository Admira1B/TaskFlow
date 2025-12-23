using AutoMapper;
using MediatR;
using TaskFlow.Tasks.Application.DTOs;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Queries.TaskItem.GetByTaskGroup {
    public class GetTaskItemsByTaskGroupQueryHandler(IMapper mapper, ITaskItemRepository repository, ITaskGroupRepository groupRepository) : IRequestHandler<GetTaskItemsByTaskGroupQuery, RequestResult<List<TaskItemDto>>> {
        private readonly IMapper _mapper = mapper;
        private readonly ITaskItemRepository _repository = repository;
        private readonly ITaskGroupRepository _groupRepository = groupRepository;

        public async Task<RequestResult<List<TaskItemDto>>> Handle(GetTaskItemsByTaskGroupQuery query, CancellationToken cancellationToken) {
            var group = await _groupRepository.GetByIdAsync(query.TaskGroupId);

            if (group is null) { 
                return RequestResult<List<TaskItemDto>>.NotFound("Task Group", query.TaskGroupId);
            }

            var tasks = await _repository.GetByTaskGroupAsync(query.TaskGroupId);

            return RequestResult<List<TaskItemDto>>.Success(_mapper.Map<List<TaskItemDto>>(tasks));
        }
    }
}
