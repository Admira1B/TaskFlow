namespace TaskFlow.Shared.Core.Interfaces {
    public interface ILogger {
        void Debug(string message, params string[] args);
        void Info(string message, params string[] args);
        void Warn(string message, params string[] args);
        void Error(string message, Exception? exception = null, params string[] args);
        void Fatal(string message, Exception? exception = null, params string[] args);
    }
}
