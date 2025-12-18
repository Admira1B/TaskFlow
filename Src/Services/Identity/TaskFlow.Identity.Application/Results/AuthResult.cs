using TaskFlow.Identity.Application.DTOs;

namespace TaskFlow.Identity.Application.Results {
    public class AuthResult {
        public bool Succeeded { get; }
        public string? ErrorMessage { get; }
        public UserDto? User { get; }

        private AuthResult(bool succeeded, UserDto? user, string? errorMessage) {
            Succeeded = succeeded;
            User = user;
            ErrorMessage = errorMessage;
        }

        public static AuthResult Success(UserDto user) {
            return new AuthResult(true, user, null);
        }

        public static AuthResult Failure(string errorMessage) {
            return new AuthResult(false, null, errorMessage);
        }
    }
}
