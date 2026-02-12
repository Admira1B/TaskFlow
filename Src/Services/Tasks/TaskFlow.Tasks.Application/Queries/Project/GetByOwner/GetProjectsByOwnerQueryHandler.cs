using MediatR;
using AutoMapper;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Contracts.DTOs.Responses;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Queries.Project.GetByOwner {
    public class GetProjectsByOwnerQueryHandler(IMapper mapper, IProjectRepository repository) : IRequestHandler<GetProjectsByOwnerQuery, RequestResult<List<ProjectDto>>> {
        private readonly IMapper _mapper = mapper;
        private readonly IProjectRepository _repository = repository;

        public async Task<RequestResult<List<ProjectDto>>> Handle(GetProjectsByOwnerQuery query, CancellationToken cancellationToken = default) {
            var projects = await _repository.GetByOwnerAsync(query.UserId, cancellationToken);

            return RequestResult<List<ProjectDto>>.Success(_mapper.Map<List<ProjectDto>>(projects));
        }
    }
}
