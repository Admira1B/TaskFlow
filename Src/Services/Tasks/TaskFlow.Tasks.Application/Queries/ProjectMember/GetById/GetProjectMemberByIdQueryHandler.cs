using AutoMapper;
using MediatR;
using TaskFlow.Tasks.Application.DTOs;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Queries.ProjectMember.GetById {
    public class GetProjectMemberByIdQueryHandler(IMapper mapper, IProjectMemberRepository repository) : IRequestHandler<GetProjectMemberByIdQuery, RequestResult<ProjectMemberDto>> {
        private readonly IMapper _mapper = mapper;
        private readonly IProjectMemberRepository _repository = repository;
        
        public async Task<RequestResult<ProjectMemberDto>> Handle(GetProjectMemberByIdQuery query, CancellationToken cancellationToken) {
            var member = await _repository.GetByIdAsync(query.Id);

            if (member is null) {
                return RequestResult<ProjectMemberDto>.NotFound("Project Member", query.Id);
            }

            return RequestResult<ProjectMemberDto>.Success(_mapper.Map<ProjectMemberDto>(member));
        }
    }
}
