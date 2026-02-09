using MediatR;
using AutoMapper;
using TaskFlow.Shared.Core.Interfaces;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Contracts.DTOs.Responses;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Commands.Project.CreateProject {
    public class CreateProjectCommandHandler(ILogger logger, IMapper mapper, IProjectRepository repository) : IRequestHandler<CreateProjectCommand, RequestResult<ProjectDto>> {
        private readonly ILogger _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IProjectRepository _repository = repository;

        public async Task<RequestResult<ProjectDto>> Handle(CreateProjectCommand command, CancellationToken cancellationToken) {
            _logger.Debug("Project creation attempt. Name: {Name}, OwnerId: {OwnerId}, DescriptionLength: {DescriptionLength}",
                command.Name,
                command.OwnerId.ToString(),
                command.Description?.Length.ToString() ?? "0"
            );

            var project = new Domain.Entities.Project(
                command.Name,
                command.OwnerId,
                command.Description
            );

            try {
                await _repository.AddAsync(project);
            } catch (Exception ex) {
                _logger.Debug("Failed to create project. Name: {Name}, OwnerId: {OwnerId}, Exception: {Message}",
                    command.Name,
                    command.OwnerId.ToString(),
                    ex.Message
                );
                return RequestResult<ProjectDto>.Failure("Failed to create project.");
            }

            _logger.Debug("Project successfully created. ProjectId: {ProjectId}, Name: {Name}, OwnerId: {OwnerId}", 
                project.Id.ToString(),
                project.Name,
                project.OwnerId.ToString()
            );

            return RequestResult<ProjectDto>.Success(_mapper.Map<ProjectDto>(project));
        }
    }
}
