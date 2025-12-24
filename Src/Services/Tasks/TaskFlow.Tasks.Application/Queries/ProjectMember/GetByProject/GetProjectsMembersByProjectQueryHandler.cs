using AutoMapper;
using MediatR;
using TaskFlow.Tasks.Application.DTOs;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Queries.ProjectMember.GetByProject {
    public class GetProjectsMembersByProjectQueryHandler(IMapper mapper, IProjectMemberRepository repository, IProjectRepository projectRepository) : IRequestHandler<GetProjectsMembersByProjectQuery, RequestResult<List<ProjectMemberDto>>> {
        private readonly IMapper _mapper = mapper;
        private readonly IProjectMemberRepository _repository = repository;
        private readonly IProjectRepository _projectRepository = projectRepository;

        public async Task<RequestResult<List<ProjectMemberDto>>> Handle(GetProjectsMembersByProjectQuery query, CancellationToken cancellationToken) {
            var project = await _projectRepository.GetByIdAsync(query.ProjectId);

            if (project is null) {
                return RequestResult<List<ProjectMemberDto>>.NotFound("Project", query.ProjectId);
            }

            var members = await _repository.GetByProjectAsync(query.ProjectId);

            return RequestResult<List<ProjectMemberDto>>.Success(_mapper.Map<List<ProjectMemberDto>>(members));
        }
    }
}
