using AutoMapper;
using MediatR;
using TaskFlow.Tasks.Application.DTOs;
using TaskFlow.Tasks.Domain.Contracts;

namespace TaskFlow.Tasks.Application.Queries.Project.GetById {
    public class GetProjectByIdQueryHandler(IMapper mapper, IProjectRepository repository) : IRequestHandler<GetProjectByIdQuery, ProjectDto?> {
        private readonly IMapper _mapper = mapper;
        private readonly IProjectRepository _repository = repository;
        
        public async Task<ProjectDto?> Handle(GetProjectByIdQuery query, CancellationToken cancellationToken) {
            var project = await _repository.GetByIdAsync(query.Id);

            if (project is null) {
                return null;
            }

            return _mapper.Map<ProjectDto>(project);
        }
    }
}
