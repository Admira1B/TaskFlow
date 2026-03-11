using MediatR;
using AutoMapper;
using TaskFlow.Shared.Core.Results;
using TaskFlow.Shared.Core.Interfaces;
using TaskFlow.Shared.ApiClients.IdentityService;
using TaskFlow.Tasks.Domain.Contracts;
using TaskFlow.Tasks.Contracts.DTOs.Responses;

namespace TaskFlow.Tasks.Application.Commands.ProjectMember.CreateProjectMember {
    public class CreateProjectMemberCommandHandler(ILogger logger, IMapper mapper, IProjectMemberRepository repository, IProjectRepository projectRepository, IdentityServiceClient client) : IRequestHandler<CreateProjectMemberCommand, RequestResult<ProjectMemberDto>> {
        private readonly ILogger _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IProjectMemberRepository _repository = repository;
        private readonly IProjectRepository _projectRepository = projectRepository;
        private readonly IdentityServiceClient _client = client;

        public async Task<RequestResult<ProjectMemberDto>> Handle(CreateProjectMemberCommand command, CancellationToken cancellationToken = default) {
            _logger.Debug("Project member creation attempt. ProjectId: {ProjectId}, UserId: {UserId}, Role: {Role}",
                command.ProjectId,
                command.UserId,
                command.Role
            );

            var userExists = await _client.UserExistsAsync(command.UserId, cancellationToken);
            if (!userExists.Exists) {
                _logger.Debug("Failed to create project member. User {UserId} not found", command.UserId);
                return RequestResult<ProjectMemberDto>.Failure($"User {command.UserId} not found");
            }

            var project = await _projectRepository.GetByIdAsync(command.ProjectId, cancellationToken);
            if (project is null) {
                _logger.Debug("Failed to create project member. Project {ProjectId} not found", command.ProjectId);
                return RequestResult<ProjectMemberDto>.Failure($"Project {command.ProjectId} not found");
            }

            if (project.OwnerId == command.UserId) {
                _logger.Debug("Failed to create project member. User {UserId} is owner of project {ProjectId}",
                    command.UserId,
                    command.ProjectId
                );
                return RequestResult<ProjectMemberDto>.Failure($"Failed to create project member. User {command.UserId} is owner of project {command.ProjectId}");
            }

            var memberExists = await _repository.UserExistsInProjectAsync(command.UserId, command.ProjectId, cancellationToken);
            if (memberExists) {
                _logger.Debug("Failed to create project member. User {UserId} already added to project {ProjectId}",
                    command.UserId,
                    command.ProjectId
                );
                return RequestResult<ProjectMemberDto>.Failure($"User {command.UserId} already added to project {command.ProjectId}");
            }

            var member = new Domain.Entities.ProjectMember() {
                ProjectId = command.ProjectId,
                UserId = command.UserId,
                Role = command.Role
            };

            try {
                await _repository.AddAsync(member, cancellationToken);
            } catch (Exception ex) {
                _logger.Debug("Failed to create project member. ProjectId: {ProjectId}, UserId: {UserId}, Exception: {Message}",
                    command.ProjectId,
                    command.UserId,
                    ex.Message
                );

                return RequestResult<ProjectMemberDto>.Failure("Failed to create project member.");
            }

            _logger.Debug("Project member successfully created. MemberId: {MemberId}, ProjectId: {ProjectId}, UserId: {UserId}, Role: {Role}",
                member.Id,
                member.ProjectId,
                member.UserId,
                member.Role.ToString()!
            );

            return RequestResult<ProjectMemberDto>.Success(_mapper.Map<ProjectMemberDto>(member));
        }
    }
}