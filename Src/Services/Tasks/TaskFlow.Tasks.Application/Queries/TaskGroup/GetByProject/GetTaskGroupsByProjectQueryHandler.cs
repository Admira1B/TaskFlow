using AutoMapper;
using MediatR;
using TaskFlow.Tasks.Application.DTOs;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Queries.TaskGroup.GetByProject {
    public class GetTaskGroupsByProjectQueryHandler(IMapper mapper, ITaskGroupRepository repository, IProjectRepository projectRepository) : IRequestHandler<GetTaskGroupsByProjectQuery, RequestResult<List<TaskGroupDto>>> {
        private readonly IMapper _mapper = mapper;
        private readonly ITaskGroupRepository _repository = repository;
        private readonly IProjectRepository _projectRepository = projectRepository;

        public async Task<RequestResult<List<TaskGroupDto>>> Handle(GetTaskGroupsByProjectQuery query, CancellationToken cancellationToken) {
            var project = await _projectRepository.GetByIdAsync(query.ProjectId);

            if (project is null) { 
                return RequestResult<List<TaskGroupDto>>.NotFound("Project", query.ProjectId);
            }

            var groups = await _repository.GetByProjectAsync(query.ProjectId);

            return RequestResult<List<TaskGroupDto>>.Success(_mapper.Map<List<TaskGroupDto>>(groups));
        }
    }
}
