using Microsoft.AspNetCore.Identity;
using TaskFlow.Identity.Domain.Entities;
using TaskFlow.Identity.Domain.Contracts.Repositories;

namespace TaskFlow.Identity.Application.Services {
    public class UserService(IUserRepository repository, UserManager<User> manager) {
        private readonly IUserRepository _repository = repository;
        private readonly UserManager<User> _manager = manager;
    }
}
