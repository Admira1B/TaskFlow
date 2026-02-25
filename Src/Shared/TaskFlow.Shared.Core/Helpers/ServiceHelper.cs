using Microsoft.Extensions.Configuration;

namespace TaskFlow.Shared.Core.Helpers {
    public static class ServiceHelper {
        public static string GetServiceName() {
            var unknownName = "not-configured-service-name";

            try {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true)
                    .AddJsonFile($"appsettings.{environment}.json", optional: true)
                    .AddEnvironmentVariables()
                    .Build();

                return config["ServiceOptions:Name"] ?? unknownName;
            } catch {
                return unknownName;
            }
        }
    }
}
