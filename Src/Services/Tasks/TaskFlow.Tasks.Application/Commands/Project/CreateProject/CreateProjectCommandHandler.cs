using AutoMapper;
using MediatR;
using TaskFlow.Tasks.Application.DTOs;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Commands.Project.CreateProject {
    public class CreateProjectCommandHandler(IMapper mapper, IProjectRepository repository) : IRequestHandler<CreateProjectCommand, ProjectDto?> {
        private readonly IMapper _mapper = mapper;
        private readonly IProjectRepository _repository = repository;

        public async Task<ProjectDto?> Handle(CreateProjectCommand command, CancellationToken cancellationToken) {
            var project = new Domain.Entities.Project(
                command.Name,
                command.OwnerId,
                command.Description
            );

            await _repository.AddAsync(project);

            return _mapper.Map<ProjectDto>(project);
        }
    }
}
