using Consul;
using Winton.Extensions.Configuration.Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Shared.Core.Options;
using TaskFlow.Shared.Consul.Options;

namespace TaskFlow.Shared.Consul.Extensions {
    public static class ConsulExtensions {
        public static IServiceCollection AddConsulConfiguration(this IServiceCollection services, WebApplicationBuilder builder) {
            var serviceOptions = builder.Configuration.GetSection(nameof(ServiceOptions)).Get<ServiceOptions>()
                ?? throw new InvalidOperationException("ServiceOptions not configured");
            var consulOptions = builder.Configuration.GetSection(nameof(ConsulOptions)).Get<ConsulOptions>()
                ?? throw new InvalidOperationException("ConsulOptions not configured");

            builder.Configuration.AddConsul($"config/{serviceOptions.Name}/{builder.Environment.EnvironmentName}", options => {
                options.ConsulConfigurationOptions = configOptions => {
                    configOptions.Address = new Uri(consulOptions.Address);
                };
                options.Optional = true;
                options.ReloadOnChange = true;
                options.PollWaitTime = TimeSpan.FromSeconds(30);
            });

            return services;
        }

        public static IServiceCollection AddConsulClient(this IServiceCollection services, WebApplicationBuilder builder) {
            var consulOptions = builder.Configuration.GetSection(nameof(ConsulOptions)).Get<ConsulOptions>()
                ?? throw new InvalidOperationException("ConsulOptions not configured");

            services.AddSingleton<IConsulClient>(sp => {
                var configuration = sp.GetRequiredService<IConfiguration>();
                return new ConsulClient(clientConfig => {
                    clientConfig.Datacenter = consulOptions.Datacenter;
                    clientConfig.Address = new Uri(consulOptions.Address);
                });
            });
            services.AddHostedService<ConsulHostedService>();

            return services;
        }
    }
}
