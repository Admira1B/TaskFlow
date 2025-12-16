using MediatR;
using Microsoft.AspNetCore.Identity;

namespace TaskFlow.Identity.Application.Commands.User.DeleteUser {
    public class DeleteUserCommandHandler(UserManager<Domain.Entities.User> manager) : IRequestHandler<DeleteUserCommand, bool> {
        private readonly UserManager<Domain.Entities.User> _manager = manager;

        public async Task<bool> Handle(DeleteUserCommand command, CancellationToken cancellationToken) {
            var user = await _manager.FindByIdAsync(command.Id.ToString());

            if (user is null) {
                return false;
            }

            var result = await _manager.DeleteAsync(user);

            if (!result.Succeeded) {
                return false;
            }

            return true;
        }
    }
}
