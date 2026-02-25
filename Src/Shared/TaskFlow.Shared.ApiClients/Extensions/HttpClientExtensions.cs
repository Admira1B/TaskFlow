using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TaskFlow.Shared.ApiClients.Extensions {
    public static class HttpClientExtensions {
        public static IServiceCollection AddServiceHttpClient<TClient>(this IServiceCollection services, IConfiguration configuration)
            where TClient : ServiceClientBase {
            var edgeServiceBaseUrl = configuration["EdgeService:BaseUrl"] ??
                    throw new InvalidOperationException(
                            "EdgeService:BaseUrl configuration is missing"
                    );

            services.AddHttpClient<TClient>(client => {
                client.BaseAddress = new Uri(edgeServiceBaseUrl);

                client.DefaultRequestHeaders.Add("Accept", "application/json");

                client.Timeout = TimeSpan.FromSeconds(30);
            })
            .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler {
                PooledConnectionLifetime = TimeSpan.FromMinutes(15),
                ConnectTimeout = TimeSpan.FromSeconds(5),
                MaxConnectionsPerServer = 20
            });

            return services;
        }
    }
}
