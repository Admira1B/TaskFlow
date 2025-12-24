using AutoMapper;
using MediatR;
using TaskFlow.Tasks.Application.DTOs;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Queries.Project.GetByOwner {
    public class GetProjectsByOwnerQueryHandler(IMapper mapper, IProjectRepository repository) : IRequestHandler<GetProjectsByOwnerQuery, RequestResult<List<ProjectDto>>> {
        private readonly IMapper _mapper = mapper;
        private readonly IProjectRepository _repository = repository;

        public async Task<RequestResult<List<ProjectDto>>> Handle(GetProjectsByOwnerQuery query, CancellationToken cancellationToken) {
            var projects = await _repository.GetByOwnerAsync(query.UserId);

            return RequestResult<List<ProjectDto>>.Success(_mapper.Map<List<ProjectDto>>(projects));
        }
    }
}
