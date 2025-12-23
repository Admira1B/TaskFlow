using AutoMapper;
using MediatR;
using TaskFlow.Tasks.Application.DTOs;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Commands.ProjectMember.CreateProjectMember {
    public class CreateProjectMemberCommandHandler(IMapper mapper, IProjectMemberRepository repository) : IRequestHandler<CreateProjectMemberCommand, RequestResult<ProjectMemberDto>> {
        private readonly IMapper _mapper = mapper;
        private readonly IProjectMemberRepository _repository = repository;
        
        public async Task<RequestResult<ProjectMemberDto>> Handle(CreateProjectMemberCommand command, CancellationToken cancellationToken) {
            var member = new Domain.Entities.ProjectMember() {
                ProjectId = command.ProjectId,
                UserId = command.UserId,
                Role = command.Role
            };

            try {
                await _repository.AddAsync(member);
            } catch (Exception) {
                return RequestResult<ProjectMemberDto>.Failure("Failed to create project member.");
            }

            return RequestResult<ProjectMemberDto>.Success(_mapper.Map<ProjectMemberDto>(member));
        }
    }
}
