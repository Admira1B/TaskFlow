using System.Reflection;

namespace TaskFlow.Shared.Core.Helpers {
    public static class ApplicationHelper {
        public static string GetApplicationVersion() { 
            var assembly = Assembly.GetEntryAssembly();
            var version = assembly?.GetName().Version?.ToString() 
                ?? throw new InvalidOperationException($"Application Version not configured");

            return version;
        }

        public static string GetMajorVersion() { 
            var version = GetApplicationVersion();
            var majorVersion = version.Split('.')[0];

            return $"v{majorVersion}";
        }
    }
}
