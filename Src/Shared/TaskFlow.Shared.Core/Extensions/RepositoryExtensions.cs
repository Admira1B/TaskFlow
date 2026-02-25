using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace TaskFlow.Shared.Core.Extensions {
    public static class RepositoryExtensions {
        public static IServiceCollection AddRepositoriesFromAssembly(this IServiceCollection services, Assembly assembly) {
            var repositoryTypes = assembly.GetTypes()
                    .Where(t => t.IsClass
                        && !t.IsAbstract
                        && t.Name.EndsWith("Repository")
                        && t.GetInterfaces().Any(i => i.Name.EndsWith("Repository")))
                    .ToList();

            foreach (var implementation in repositoryTypes) {
                var interfaces = implementation.GetInterfaces()
                    .Where(i => i.Name.EndsWith("Repository"))
                    .ToList();

                foreach (var @interface in interfaces) {
                    services.TryAddScoped(@interface, implementation);
                }
            }

            return services;
        }
    }
}
