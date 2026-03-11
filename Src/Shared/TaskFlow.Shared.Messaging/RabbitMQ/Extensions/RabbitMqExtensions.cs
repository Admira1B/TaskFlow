using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Shared.Messaging.RabbitMQ.Services;

namespace TaskFlow.Shared.Messaging.RabbitMQ.Extensions {
    public static class RabbitMqExtensions {
        public static IServiceCollection AddRabbitMqEventPublisher<TInterface, TImplementation>(this IServiceCollection services)
            where TInterface : class 
            where TImplementation : RabbitMqEventPublisher, TInterface{
            services.AddSingleton<TInterface, TImplementation>();

            return services;
        }

        public static IServiceCollection AddRabbitMqEventConsumer<TConsumer>(this IServiceCollection services)
            where TConsumer : RabbitMqEventConsumer {
            services.AddHostedService<TConsumer>();
            
            return services;
        }
    }
}
