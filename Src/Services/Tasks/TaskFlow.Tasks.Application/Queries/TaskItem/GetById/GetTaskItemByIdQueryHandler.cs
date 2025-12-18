using AutoMapper;
using MediatR;
using TaskFlow.Tasks.Application.DTOs;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Queries.TaskItem.GetById {
    public class GetTaskItemByIdQueryHandler(IMapper mapper, ITaskItemRepository repository) : IRequestHandler<GetTaskItemByIdQuery, TaskItemDto?> {
        private readonly IMapper _mapper = mapper;
        private readonly ITaskItemRepository _repository = repository;

        public async Task<TaskItemDto?> Handle(GetTaskItemByIdQuery request, CancellationToken cancellationToken) {
            var task = await _repository.GetByIdAsync(request.Id);

            if (task is null) {
                return null;
            }

            return _mapper.Map<TaskItemDto>(task);
        }
    }
}
