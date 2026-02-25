using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace TaskFlow.Shared.Core.Extensions {
    public static class MediatorExtensions {
        public static IServiceCollection AddMediator(this IServiceCollection services, Assembly assembly) {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(assembly)
            );

            return services;
        }
    }
}
