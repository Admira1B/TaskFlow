using MediatR;
using AutoMapper;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Contracts.DTOs.Responses;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Queries.TaskItem.GetByAssignee {
    public class GetTaskItemsByAssigneeQueryHandler(IMapper mapper, ITaskItemRepository repository) : IRequestHandler<GetTaskItemsByAssigneeQuery, RequestResult<List<TaskItemDto>>> {
        private readonly IMapper _mapper = mapper;
        private readonly ITaskItemRepository _repository = repository;

        public async Task<RequestResult<List<TaskItemDto>>> Handle(GetTaskItemsByAssigneeQuery query, CancellationToken cancellationToken = default) {
            var tasks = await _repository.GetByAssigneeAsync(query.UserId, cancellationToken);

            return RequestResult<List<TaskItemDto>>.Success(_mapper.Map<List<TaskItemDto>>(tasks));
        }
    }
}
