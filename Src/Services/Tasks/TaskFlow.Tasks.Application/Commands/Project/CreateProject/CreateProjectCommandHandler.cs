using AutoMapper;
using MediatR;
using TaskFlow.Tasks.Application.DTOs;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Commands.Project.CreateProject {
    public class CreateProjectCommandHandler(IMapper mapper, IProjectRepository repository) : IRequestHandler<CreateProjectCommand, RequestResult<ProjectDto>> {
        private readonly IMapper _mapper = mapper;
        private readonly IProjectRepository _repository = repository;

        public async Task<RequestResult<ProjectDto>> Handle(CreateProjectCommand command, CancellationToken cancellationToken) {
            var project = new Domain.Entities.Project(
                command.Name,
                command.OwnerId,
                command.Description
            );

            try {
                await _repository.AddAsync(project);
            } catch (Exception) {
                return RequestResult<ProjectDto>.Failure("Failed to create project.");
            }

            return RequestResult<ProjectDto>.Success(_mapper.Map<ProjectDto>(project));
        }
    }
}
