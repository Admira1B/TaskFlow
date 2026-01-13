using MediatR;
using AutoMapper;
using TaskFlow.Tasks.Domain.Contracts;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Application.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Queries.TaskItem.GetByAssignee {
    public class GetTaskItemsByAssigneeQueryHandler(IMapper mapper, ITaskItemRepository repository) : IRequestHandler<GetTaskItemsByAssigneeQuery, RequestResult<List<TaskItemDto>>> {
        private readonly IMapper _mapper = mapper;
        private readonly ITaskItemRepository _repository = repository;

        public async Task<RequestResult<List<TaskItemDto>>> Handle(GetTaskItemsByAssigneeQuery query, CancellationToken cancellationToken) {
            var tasks = await _repository.GetByAssigneeAsync(query.UserId);

            return RequestResult<List<TaskItemDto>>.Success(_mapper.Map<List<TaskItemDto>>(tasks));
        }
    }
}
