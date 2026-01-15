using MediatR;
using AutoMapper;
using TaskFlow.Tasks.Domain.Contracts;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Contracts.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Queries.TaskItem.GetByProject {
    public class GetTaskItemsByProjectQueryHandler(IMapper mapper, ITaskItemRepository repository, IProjectRepository projectRepository) : IRequestHandler<GetTaskItemsByProjectQuery, RequestResult<List<TaskItemDto>>> {
        private readonly IMapper _mapper = mapper;
        private readonly ITaskItemRepository _repository = repository;
        private readonly IProjectRepository _projectRepository = projectRepository;

        public async Task<RequestResult<List<TaskItemDto>>> Handle(GetTaskItemsByProjectQuery query, CancellationToken cancellationToken) {
            var project = await _projectRepository.GetByIdAsync(query.ProjectId);

            if (project is null) {
                return RequestResult<List<TaskItemDto>>.NotFound("Project", query.ProjectId);
            }

            var tasks = await _repository.GetByProjectAsync(query.ProjectId);

            return RequestResult<List<TaskItemDto>>.Success(_mapper.Map<List<TaskItemDto>>(tasks));
        }
    }
}
