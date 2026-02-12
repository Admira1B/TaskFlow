using MediatR;
using AutoMapper;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Contracts.DTOs.Responses;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Queries.ProjectMember.GetByProject {
    public class GetProjectsMembersByProjectQueryHandler(IMapper mapper, IProjectMemberRepository repository, IProjectRepository projectRepository) : IRequestHandler<GetProjectsMembersByProjectQuery, RequestResult<List<ProjectMemberDto>>> {
        private readonly IMapper _mapper = mapper;
        private readonly IProjectMemberRepository _repository = repository;
        private readonly IProjectRepository _projectRepository = projectRepository;

        public async Task<RequestResult<List<ProjectMemberDto>>> Handle(GetProjectsMembersByProjectQuery query, CancellationToken cancellationToken = default) {
            var project = await _projectRepository.GetByIdAsync(query.ProjectId, cancellationToken);

            if (project is null) {
                return RequestResult<List<ProjectMemberDto>>.NotFound("Project", query.ProjectId);
            }

            var members = await _repository.GetByProjectAsync(query.ProjectId, cancellationToken);

            return RequestResult<List<ProjectMemberDto>>.Success(_mapper.Map<List<ProjectMemberDto>>(members));
        }
    }
}
