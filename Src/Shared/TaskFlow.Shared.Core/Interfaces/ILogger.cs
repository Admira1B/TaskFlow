namespace TaskFlow.Shared.Core.Interfaces {
    public interface ILogger {
        void Debug(string message, params object[] args);
        void Info(string message, params object[] args);
        void Warn(string message, params object[] args);
        void Error(string message, Exception? exception = null, params object[] args);
        void Fatal(string message, Exception? exception = null, params object[] args);
    }
}
