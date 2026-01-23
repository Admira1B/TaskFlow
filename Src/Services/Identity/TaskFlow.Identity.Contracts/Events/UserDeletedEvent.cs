using MediatR;
using TaskFlow.Shared.Core.Entities;

namespace TaskFlow.Identity.Contracts.Events {
    public class UserDeletedEvent : EventBase, INotification {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
