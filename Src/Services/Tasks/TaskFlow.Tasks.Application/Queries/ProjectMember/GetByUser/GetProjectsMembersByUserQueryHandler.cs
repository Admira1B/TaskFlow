using MediatR;
using AutoMapper;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Contracts.DTOs.Responses;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Queries.ProjectMember.GetByUser {
    public class GetProjectsMembersByUserQueryHandler(IMapper mapper, IProjectMemberRepository repository) : IRequestHandler<GetProjectsMembersByUserQuery, RequestResult<List<ProjectMemberDto>>> {
        private readonly IMapper _mapper = mapper;
        private readonly IProjectMemberRepository _repository = repository;

        public async Task<RequestResult<List<ProjectMemberDto>>> Handle(GetProjectsMembersByUserQuery query, CancellationToken cancellationToken = default) {
            var members = await _repository.GetByUserAsync(query.UserId, cancellationToken);

            return RequestResult<List<ProjectMemberDto>>.Success(_mapper.Map<List<ProjectMemberDto>>(members));
        }
    }
}
