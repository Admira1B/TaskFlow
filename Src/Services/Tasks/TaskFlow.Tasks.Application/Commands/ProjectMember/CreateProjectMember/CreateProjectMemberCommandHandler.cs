using MediatR;
using AutoMapper;
using TaskFlow.Shared.Core.Interfaces;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Contracts.DTOs.Responses;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Commands.ProjectMember.CreateProjectMember {
    public class CreateProjectMemberCommandHandler(ILogger logger, IMapper mapper, IProjectMemberRepository repository) : IRequestHandler<CreateProjectMemberCommand, RequestResult<ProjectMemberDto>> {
        private readonly ILogger _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IProjectMemberRepository _repository = repository;

        public async Task<RequestResult<ProjectMemberDto>> Handle(CreateProjectMemberCommand command, CancellationToken cancellationToken) {
            _logger.Debug("Project member creation attempt. ProjectId: {ProjectId}, UserId: {UserId}, Role: {Role}",
                command.ProjectId.ToString(),
                command.UserId.ToString(),
                command.Role.ToString()
            );

            var member = new Domain.Entities.ProjectMember() {
                ProjectId = command.ProjectId,
                UserId = command.UserId,
                Role = command.Role
            };

            try {
                await _repository.AddAsync(member);
            } catch (Exception ex) {
                _logger.Debug("Failed to create project member. ProjectId: {ProjectId}, UserId: {UserId}, Exception: {Message}",
                    command.ProjectId.ToString(),
                    command.UserId.ToString(),
                    ex.Message
                );

                return RequestResult<ProjectMemberDto>.Failure("Failed to create project member.");
            }

            _logger.Debug("Project member successfully created. MemberId: {MemberId}, ProjectId: {ProjectId}, UserId: {UserId}, Role: {Role}",
                member.Id.ToString(),
                member.ProjectId.ToString(),
                member.UserId.ToString(),
                member.Role.ToString()!
            );

            return RequestResult<ProjectMemberDto>.Success(_mapper.Map<ProjectMemberDto>(member));
        }
    }
}