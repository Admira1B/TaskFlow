using MediatR;
using Microsoft.AspNetCore.Identity;

namespace TaskFlow.Identity.Application.Commands.User.UpdateUser {
    public class UpdateUserCommandHandler(UserManager<Domain.Entities.User> manager) : IRequestHandler<UpdateUserCommand, bool> {
        private readonly UserManager<Domain.Entities.User> _manager = manager;
        
        public async Task<bool> Handle(UpdateUserCommand command, CancellationToken cancellationToken) {
            var user = await _manager.FindByIdAsync(command.Id.ToString());

            if (user is null) {
                return false;
            }

            user.FirstName = command.FirstName;
            user.LastName = command.LastName;
            user.UpdatedAt = DateTime.UtcNow;

            var result = await _manager.UpdateAsync(user);

            if (!result.Succeeded) {
                return false;
            }

            return true;
        }
    }
}
