using MediatR;
using TaskFlow.Shared.Core.Enums;

namespace TaskFlow.Shared.Core.Results {
    public class RequestResult<T> {
        public bool Succeeded { get; }
        public ErrorType ErrorType { get; }
        public T? Value { get; }
        public string? ErrorMessage { get; }

        private RequestResult(bool succeeded, ErrorType errorType, T? value, string? errorMessage) {
            Succeeded = succeeded;
            ErrorType = errorType;
            Value = value;
            ErrorMessage = errorMessage;
        }

        public static RequestResult<T> Success(T value) {
            return new RequestResult<T>(true, ErrorType.Ok, value, null);
        }

        public static RequestResult<Unit> Success() {
            return new RequestResult<Unit>(true, ErrorType.Ok, Unit.Value, null);
        }

        public static RequestResult<T> Failure(string errorMessage, ErrorType errorType = ErrorType.ValidationFailed) {
            return new RequestResult<T>(false, errorType, default, errorMessage);
        }

        public static RequestResult<T> NotFound(string entityName, Guid? id = null) {
            var message = id.HasValue
                ? $"{entityName} with ID {id} not found"
                : $"{entityName} not found";

            return Failure(message, ErrorType.EntityNotFound);
        }

        public static RequestResult<T> AlreadyExists(string entityName, string identifier) {
            return Failure($"{entityName} '{identifier}' already exists", ErrorType.AlreadyExists);
        }

        public static RequestResult<T> ValidationError(string message) {
            return Failure(message, ErrorType.ValidationFailed);
        }

        public static RequestResult<T> InvalidOperation(string message) {
            return Failure(message, ErrorType.InvalidOperation);
        }

        public static RequestResult<T> FailedToPublishEvent(string message) {
            return Failure(message, ErrorType.FailedToPublishEvent);
        }

        public void Deconstruct(out bool succeeded, out T? value, out string? errorMessage, out ErrorType errorType) {
            succeeded = Succeeded;
            value = Value;
            errorMessage = ErrorMessage;
            errorType = ErrorType;
        }

        public bool TryGetValue(out T? value) {
            value = Value;
            return Succeeded;
        }
    }
}
