using MediatR;
using AutoMapper;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Contracts.DTOs.Responses;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Queries.TaskItem.GetByReporter {
    public class GetTaskItemsByReporterQueryHandler(IMapper mapper, ITaskItemRepository repository) : IRequestHandler<GetTaskItemsByReporterQuery, RequestResult<List<TaskItemDto>>> {
        private readonly IMapper _mapper = mapper;
        private readonly ITaskItemRepository _repository = repository;

        public async Task<RequestResult<List<TaskItemDto>>> Handle(GetTaskItemsByReporterQuery query, CancellationToken cancellationToken = default) {
            var tasks = await _repository.GetByReporterAsync(query.UserId, cancellationToken);

            return RequestResult<List<TaskItemDto>>.Success(_mapper.Map<List<TaskItemDto>>(tasks));
        }
    }
}
