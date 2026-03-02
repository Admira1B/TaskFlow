using MediatR;
using AutoMapper;
using TaskFlow.Shared.Core.Results;
using TaskFlow.Tasks.Domain.Contracts;
using TaskFlow.Tasks.Contracts.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Queries.TaskGroup.GetByProject {
    public class GetTaskGroupsByProjectQueryHandler(IMapper mapper, ITaskGroupRepository repository, IProjectRepository projectRepository) : IRequestHandler<GetTaskGroupsByProjectQuery, RequestResult<List<TaskGroupDto>>> {
        private readonly IMapper _mapper = mapper;
        private readonly ITaskGroupRepository _repository = repository;
        private readonly IProjectRepository _projectRepository = projectRepository;

        public async Task<RequestResult<List<TaskGroupDto>>> Handle(GetTaskGroupsByProjectQuery query, CancellationToken cancellationToken = default) {
            var project = await _projectRepository.GetByIdAsync(query.ProjectId, cancellationToken);

            if (project is null) { 
                return RequestResult<List<TaskGroupDto>>.NotFound("Project", query.ProjectId);
            }

            var groups = await _repository.GetByProjectAsync(query.ProjectId, cancellationToken);

            return RequestResult<List<TaskGroupDto>>.Success(_mapper.Map<List<TaskGroupDto>>(groups));
        }
    }
}
