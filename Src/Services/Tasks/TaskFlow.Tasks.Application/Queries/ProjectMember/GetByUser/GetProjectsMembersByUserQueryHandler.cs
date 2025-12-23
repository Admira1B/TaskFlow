using AutoMapper;
using MediatR;
using TaskFlow.Tasks.Application.DTOs;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Queries.ProjectMember.GetByUser {
    public class GetProjectsMembersByUserQueryHandler(IMapper mapper, IProjectMemberRepository repository) : IRequestHandler<GetProjectsMembersByUserQuery, RequestResult<List<ProjectMemberDto>>> {
        private readonly IMapper _mapper = mapper;
        private readonly IProjectMemberRepository _repository = repository;

        public async Task<RequestResult<List<ProjectMemberDto>>> Handle(GetProjectsMembersByUserQuery query, CancellationToken cancellationToken) {
            var members = await _repository.GetByUserAsync(query.UserId);

            return RequestResult<List<ProjectMemberDto>>.Success(_mapper.Map<List<ProjectMemberDto>>(members));
        }
    }
}
