namespace TaskFlow.Identity.Domain.Enums {
    public enum ErrorType {
        Ok = 0,
        ValidationFailed = 1,
        InvalidOperation = 2,
        EntityNotFound = 3,
        AlreadyExists = 4,
        InvalidCredentials = 5,
    }
}
