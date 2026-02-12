using MediatR;
using AutoMapper;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Contracts.DTOs.Responses;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Queries.TaskGroup.GetById {
    public class GetTaskGroupByIdQueryHandler(IMapper mapper, ITaskGroupRepository repository) : IRequestHandler<GetTaskGroupByIdQuery, RequestResult<TaskGroupDto>>{
        private readonly IMapper _mapper = mapper;
        private readonly ITaskGroupRepository _repository = repository;

        public async Task<RequestResult<TaskGroupDto>> Handle(GetTaskGroupByIdQuery query, CancellationToken cancellationToken = default) {
            var group = await _repository.GetByIdAsync(query.Id, cancellationToken);

            if (group is null) {
                return RequestResult<TaskGroupDto>.NotFound("Task Group", query.Id);
            }

            return RequestResult<TaskGroupDto>.Success(_mapper.Map<TaskGroupDto>(group));
        }
    }
}
