using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Identity.Application.Results;
using TaskFlow.Identity.Contracts.DTOs.Responses;

namespace TaskFlow.Identity.Application.Queries.User.Exists {
    public class UserExistsQueryHandler(UserManager<Domain.Entities.User> manager) : IRequestHandler<UserExistsQuery, RequestResult<ExistenceResponse>> {
        private readonly UserManager<Domain.Entities.User> _manager = manager;

        public async Task<RequestResult<ExistenceResponse>> Handle(UserExistsQuery query, CancellationToken cancellationToken = default) {
            try {
                var result = await _manager.Users.AnyAsync(u => u.Id == query.UserId, cancellationToken: cancellationToken);

                return RequestResult<ExistenceResponse>.Success(new ExistenceResponse(result));
            } catch (Exception) {
                return RequestResult<ExistenceResponse>.Failure($"Failed to check for existence user {query.UserId}");
            }
        }
    }
}
