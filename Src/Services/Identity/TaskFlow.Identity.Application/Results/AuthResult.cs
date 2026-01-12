using TaskFlow.Identity.Application.DTOs.Responses;

namespace TaskFlow.Identity.Application.Results {
    public class AuthResult {
        public bool Succeeded { get; }
        public string? ErrorMessage { get; }
        public UserDto? User { get; }
        public string? Token { get; }

        private AuthResult(bool succeeded, UserDto? user, string? token, string? errorMessage) {
            Succeeded = succeeded;
            User = user;
            Token = token;
            ErrorMessage = errorMessage;
        }

        public static AuthResult Success(UserDto user, string token) {
            return new AuthResult(true, user, token, null);
        }

        public static AuthResult Failure(string errorMessage) {
            return new AuthResult(false, null, null, errorMessage);
        }
    }
}
