using MediatR;
using TaskFlow.Tasks.Domain.Enums;

namespace TaskFlow.Tasks.Application.Results {
    public class RequestResult<T> {
        public bool Succeeded { get; }
        public T? Value { get; }
        public string? ErrorMessage { get; }
        public ErrorEnum ErrorType { get; }

        protected RequestResult(bool succeeded, T? value, string? errorMessage, ErrorEnum errorType) {
            Succeeded = succeeded;
            Value = value;
            ErrorMessage = errorMessage;
            ErrorType = errorType;
        }

        public static RequestResult<T> Success(T value) {
            return new RequestResult<T>(true, value, null, ErrorEnum.Ok);
        }

        // For methods that returns void
        public static RequestResult<Unit> Success() {
            return new RequestResult<Unit>(true, Unit.Value, null, ErrorEnum.Ok);
        }

        public static RequestResult<T> Failure(string errorMessage, ErrorEnum errorType = ErrorEnum.ValidationFailed) {
            return new RequestResult<T>(false, default, errorMessage, errorType);
        }

        public static RequestResult<T> NotFound(string entityName, Guid? id = null) {
            var message = id.HasValue
                ? $"{entityName} with ID {id} not found"
                : $"{entityName} not found";

            return Failure(message, ErrorEnum.EntityNotFound);
        }

        public static RequestResult<T> AlreadyExists(string entityName, string identifier) {
            return Failure($"{entityName} '{identifier}' already exists", ErrorEnum.AlreadyExists);
        }

        public static RequestResult<T> ValidationError(string message) {
            return Failure(message, ErrorEnum.ValidationFailed);
        }

        public static RequestResult<T> InvalidOperation(string message) {
            return Failure(message, ErrorEnum.InvalidOperation);
        }

        public void Deconstruct(out bool succeeded, out T? value, out string? errorMessage, out ErrorEnum errorType) {
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
