using MediatR;
using AutoMapper;
using TaskFlow.Tasks.Domain.Contracts;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Application.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Commands.TaskItem.CreateTaskItem {
    public class CreateTaskItemCommandHandler(IMapper mapper, ITaskItemRepository repository) : IRequestHandler<CreateTaskItemCommand, RequestResult<TaskItemDto>> {
        private readonly IMapper _mapper = mapper;
        private readonly ITaskItemRepository _repository = repository;
        
        public async Task<RequestResult<TaskItemDto>> Handle(CreateTaskItemCommand command, CancellationToken cancellationToken) {
            var task = new Domain.Entities.TaskItem(
                command.Title,
                command.GroupId, 
                command.ReporterId, 
                command.Description, 
                command.AssignedId, 
                command.Priority
            );

            try {
                await _repository.AddAsync(task);
            } catch (Exception) {
                return RequestResult<TaskItemDto>.Failure("Failed to create task.");
            }

            return RequestResult<TaskItemDto>.Success(_mapper.Map<TaskItemDto>(task));
        }
    }
}
