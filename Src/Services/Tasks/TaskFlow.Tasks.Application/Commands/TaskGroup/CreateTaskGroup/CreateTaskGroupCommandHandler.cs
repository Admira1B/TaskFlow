using AutoMapper;
using MediatR;
using TaskFlow.Tasks.Application.DTOs;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Commands.TaskGroup.CreateTaskGroup {
    public class CreateTaskGroupCommandHandler(IMapper mapper, ITaskGroupRepository repository) : IRequestHandler<CreateTaskGroupCommand, RequestResult<TaskGroupDto>> {
        private readonly IMapper _mapper = mapper;
        private readonly ITaskGroupRepository _repository = repository;
        
        public async Task<RequestResult<TaskGroupDto>> Handle(CreateTaskGroupCommand command, CancellationToken cancellationToken) {
            var group = new Domain.Entities.TaskGroup() {
                ProjectId = command.ProjectId,
                Name = command.Name
            };

            try {
                await _repository.AddAsync(group);
            } catch (Exception) {
                return RequestResult<TaskGroupDto>.Failure("Failed to create task group.");
            }

            return RequestResult<TaskGroupDto>.Success(_mapper.Map<TaskGroupDto>(group));
        }
    }
}
