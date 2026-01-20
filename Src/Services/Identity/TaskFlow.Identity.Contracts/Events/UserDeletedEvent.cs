using MediatR;
using TaskFlow.Shared.Messaging;

namespace TaskFlow.Identity.Contracts.Events {
    public class UserDeletedEvent : BaseEvent, INotification {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
