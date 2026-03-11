using MediatR;
using AutoMapper;
using TaskFlow.Shared.Core.Results;
using TaskFlow.Shared.Core.Interfaces;
using TaskFlow.Tasks.Domain.Contracts;
using TaskFlow.Tasks.Contracts.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Commands.TaskGroup.CreateTaskGroup {
    public class CreateTaskGroupCommandHandler(ILogger logger, IMapper mapper, ITaskGroupRepository repository) : IRequestHandler<CreateTaskGroupCommand, RequestResult<TaskGroupDto>> {
        private readonly ILogger _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly ITaskGroupRepository _repository = repository;

        public async Task<RequestResult<TaskGroupDto>> Handle(CreateTaskGroupCommand command, CancellationToken cancellationToken = default) {
            _logger.Debug("Task group creation attempt. ProjectId: {ProjectId}, Name: {Name}",
                command.ProjectId,
                command.Name
            );

            var group = new Domain.Entities.TaskGroup() {
                ProjectId = command.ProjectId,
                Name = command.Name
            };

            try {
                await _repository.AddAsync(group, cancellationToken);
            } catch (Exception ex) {
                _logger.Debug("Failed to create task group. ProjectId: {ProjectId}, Name: {Name}, Exception: {Message}",
                    command.ProjectId,
                    command.Name,
                    ex.Message
                );

                return RequestResult<TaskGroupDto>.Failure("Failed to create task group.");
            }

            _logger.Debug("Task group successfully created. GroupId: {GroupId}, ProjectId: {ProjectId}, Name: {Name}",
                group.Id,
                group.ProjectId,
                group.Name
            );

            return RequestResult<TaskGroupDto>.Success(_mapper.Map<TaskGroupDto>(group));
        }
    }
}