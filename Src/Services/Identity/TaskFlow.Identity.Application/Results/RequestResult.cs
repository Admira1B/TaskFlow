using MediatR;

namespace TaskFlow.Identity.Application.Results {
    public class RequestResult<T> {
        public bool Succeeded { get; }
        public T? Value { get; }
        public string? ErrorMessage { get; }

        private RequestResult(bool succeeded, T? value, string? errorMessage) {
            Succeeded = succeeded; 
            Value = value; 
            ErrorMessage = errorMessage;
        }

        public static RequestResult<T> Success(T value) {
            return new RequestResult<T>(true, value, null);
        }

        public static RequestResult<Unit> Success() { 
            return new RequestResult<Unit>(true, Unit.Value, null);
        }

        public static RequestResult<T> Failure(string errorMessage) { 
            return new RequestResult<T>(false, default, errorMessage);
        }
    }
}
