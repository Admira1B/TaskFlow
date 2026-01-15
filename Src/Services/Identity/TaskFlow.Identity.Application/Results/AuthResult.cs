using TaskFlow.Identity.Domain.Enums;
using TaskFlow.Identity.Contracts.DTOs.Responses;

namespace TaskFlow.Identity.Application.Results {
    public class AuthResult {
        public bool Succeeded { get; }
        public ErrorType ErrorType { get; }
        public string? ErrorMessage { get; }
        public UserDto? User { get; }
        public string? Token { get; }

        private AuthResult(bool succeeded, ErrorType errorType, UserDto? user, string? token, string? errorMessage) {
            Succeeded = succeeded;
            ErrorType = errorType;
            User = user;
            Token = token;
            ErrorMessage = errorMessage;
        }

        public static AuthResult Success(UserDto user, string token) {
            return new AuthResult(true, ErrorType.Ok, user, token, null);
        }

        public static AuthResult Failure(string errorMessage, ErrorType errorType = ErrorType.ValidationFailed) {
            return new AuthResult(false, errorType, null, null, errorMessage);
        }

        public static AuthResult NotFound(string entityName, string value) {
            return Failure($"User with {entityName} '{value}' was not found", ErrorType.EntityNotFound);
        }

        public static AuthResult AlreadyExists(string entityName, string value) {
            return Failure($"User with {entityName} '{value}' already exists", ErrorType.AlreadyExists);
        }

        public static AuthResult InvalidCredentials(string? errorMessage = null) { 
            return Failure(errorMessage ?? $"Invalid credentials", ErrorType.InvalidCredentials);
        }
    }
}
