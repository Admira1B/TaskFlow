using MediatR;
using AutoMapper;
using TaskFlow.Shared.Core.Results;
using TaskFlow.Shared.Core.Interfaces;
using TaskFlow.Tasks.Domain.Contracts;
using TaskFlow.Tasks.Contracts.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Commands.TaskItem.CreateTaskItem {
    public class CreateTaskItemCommandHandler(ILogger logger, IMapper mapper, ITaskItemRepository repository) : IRequestHandler<CreateTaskItemCommand, RequestResult<TaskItemDto>> {
        private readonly ILogger _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly ITaskItemRepository _repository = repository;

        public async Task<RequestResult<TaskItemDto>> Handle(CreateTaskItemCommand command, CancellationToken cancellationToken = default) {
            _logger.Debug("Task item creation attempt. Title: {Title}, GroupId: {GroupId}, ReporterId: {ReporterId}, AssignedId: {AssignedId}, Priority: {Priority}, DescriptionLength: {DescriptionLength}",
                command.Title,
                command.GroupId,
                command.ReporterId,
                command.AssignedId?.ToString() ?? "null",
                command.Priority,
                command.Description?.Length.ToString() ?? "0"
            );

            var task = new Domain.Entities.TaskItem(
                command.Title,
                command.GroupId,
                command.ReporterId,
                command.Description,
                command.AssignedId,
                command.Priority
            );

            try {
                await _repository.AddAsync(task, cancellationToken);
            } catch (Exception ex) {
                _logger.Debug("Failed to create task item. Title: {Title}, GroupId: {GroupId}, Exception: {Message}",
                    command.Title,
                    command.GroupId,
                    ex.Message
                );
                return RequestResult<TaskItemDto>.Failure("Failed to create task.");
            }

            _logger.Debug("Task item successfully created. TaskId: {TaskId}, Title: {Title}, GroupId: {GroupId}, ReporterId: {ReporterId}",
                task.Id,
                task.Title,
                task.GroupId,
                task.ReporterId
            );

            return RequestResult<TaskItemDto>.Success(_mapper.Map<TaskItemDto>(task));
        }
    }
}