using AutoMapper;
using MediatR;
using TaskFlow.Tasks.Application.DTOs;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Queries.TaskGroup.GetById {
    public class GetTaskGroupByIdQueryHandler(IMapper mapper, ITaskGroupRepository repository) : IRequestHandler<GetTaskGroupByIdQuery, RequestResult<TaskGroupDto>>{
        private readonly IMapper _mapper = mapper;
        private readonly ITaskGroupRepository _repository = repository;

        public async Task<RequestResult<TaskGroupDto>> Handle(GetTaskGroupByIdQuery query, CancellationToken cancellationToken) {
            var group = await _repository.GetByIdAsync(query.Id);

            if (group is null) {
                return RequestResult<TaskGroupDto>.NotFound("Task Group", query.Id);
            }

            return RequestResult<TaskGroupDto>.Success(_mapper.Map<TaskGroupDto>(group));
        }
    }
}
