using AutoMapper;
using MediatR;
using TaskFlow.Tasks.Application.DTOs;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Queries.TaskGroup.GetByIdWithTasks {
    public class GetTaskGroupWithTasksByIdQueryHandler(IMapper mapper, ITaskGroupRepository repository) : IRequestHandler<GetTaskGroupWithTasksByIdQuery, RequestResult<TaskGroupDto>> {
        private readonly IMapper _mapper = mapper;
        private readonly ITaskGroupRepository _repository = repository;

        public Task<RequestResult<TaskGroupDto>> Handle(GetTaskGroupWithTasksByIdQuery query, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }
    }
}
