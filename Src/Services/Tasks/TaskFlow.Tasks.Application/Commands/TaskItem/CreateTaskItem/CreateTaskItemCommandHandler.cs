using AutoMapper;
using MediatR;
using TaskFlow.Tasks.Application.DTOs;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Commands.TaskItem.CreateTaskItem {
    public class CreateTaskItemCommandHandler(IMapper mapper, ITaskItemRepository repository) : IRequestHandler<CreateTaskItemCommand, TaskItemDto?> {
        private readonly IMapper _mapper = mapper;
        private readonly ITaskItemRepository _repository = repository;
        
        public async Task<TaskItemDto?> Handle(CreateTaskItemCommand command, CancellationToken cancellationToken) {
            var task = new Domain.Entities.TaskItem(
                command.Title,
                command.GroupId, 
                command.ReporterId, 
                command.Description, 
                command.AssignedId, 
                command.Priority
            );

            await _repository.AddAsync(task);

            return _mapper.Map<TaskItemDto>(task);
        }
    }
}
