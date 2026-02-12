using MediatR;
using AutoMapper;
using TaskFlow.Tasks.Application.Results;
using TaskFlow.Tasks.Contracts.DTOs.Responses;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Queries.Project.GetById {
    public class GetProjectByIdQueryHandler(IMapper mapper, IProjectRepository repository) : IRequestHandler<GetProjectByIdQuery, RequestResult<ProjectDto>> {
        private readonly IMapper _mapper = mapper;
        private readonly IProjectRepository _repository = repository;
        
        public async Task<RequestResult<ProjectDto>> Handle(GetProjectByIdQuery query, CancellationToken cancellationToken = default) {
            var project = await _repository.GetByIdAsync(query.Id, cancellationToken);

            if (project is null) {
                return RequestResult<ProjectDto>.NotFound("Project", query.Id);
            }

            return RequestResult<ProjectDto>.Success(_mapper.Map<ProjectDto>(project));
        }
    }
}
