using MediatR;
using AutoMapper;
using TaskFlow.Shared.Core.Results;
using TaskFlow.Tasks.Contracts.DTOs.Responses;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Queries.Project.GetByProjectMember {
    public class GetProjectsByProjectMemberQueryHandler(IMapper mapper, IProjectRepository repository) : IRequestHandler<GetProjectsByProjectMemberQuery, RequestResult<List<ProjectDto>>> {
        private readonly IMapper _mapper = mapper;
        private readonly IProjectRepository _repository = repository;

        public async Task<RequestResult<List<ProjectDto>>> Handle(GetProjectsByProjectMemberQuery query, CancellationToken cancellationToken = default) {
            var projects = await _repository.GetByProjectMemberAsync(query.UserId, cancellationToken);

            return RequestResult<List<ProjectDto>>.Success(_mapper.Map<List<ProjectDto>>(projects));
        }
    }
}
