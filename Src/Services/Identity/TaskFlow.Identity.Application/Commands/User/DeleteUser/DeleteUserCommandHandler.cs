using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskFlow.Identity.Application.Results;

namespace TaskFlow.Identity.Application.Commands.User.DeleteUser {
    public class DeleteUserCommandHandler(UserManager<Domain.Entities.User> manager) : IRequestHandler<DeleteUserCommand, RequestResult<Unit>> {
        private readonly UserManager<Domain.Entities.User> _manager = manager;

        public async Task<RequestResult<Unit>> Handle(DeleteUserCommand command, CancellationToken cancellationToken) {
            var user = await _manager.FindByIdAsync(command.Id.ToString());

            if (user is null) {
                return RequestResult<Unit>.NotFound("User", command.Id);
            }

            var result = await _manager.DeleteAsync(user);

            if (!result.Succeeded) {
                var errors = result.Errors.Select(e => e.Description);

                return RequestResult<Unit>.Failure(string.Join(",", errors));
            }

            return RequestResult<Unit>.Success();
        }
    }
}
