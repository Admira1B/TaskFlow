using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using TaskFlow.Tasks.Infrastructure.SqlServer;

namespace TaskFlow.Tasks.API {
    internal static class ServiceCollectionExtensions {
        public static IServiceCollection AddTaskServiceDependencies(this IServiceCollection services) {
            // Adding controllers
            services.AddControllers();

            // Adding documentation
            services.AddOpenApi();
            services.AddSwaggerGen(options =>
                options.SwaggerDoc("v1", new OpenApiInfo {
                    Version = "v1",
                    Title = "TaskFlow Tasks Service",
                    Contact = new OpenApiContact {
                        Name = "Vlad Reizenbuk", Email = "vreizenbuk@mail.ru"
                    }
                })
            );

            // DbContext
            services.AddDbContext<TaskServiceDbContext>((serviceProvider, options) => {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("SqlServerConnectionString");

                options.UseSqlServer(connectionString, sqlOptions => {
                    sqlOptions.MigrationsAssembly(typeof(TaskServiceDbContext).Assembly.FullName);
                    sqlOptions.EnableRetryOnFailure();
                });
            });

            return services;
        }
    }
}
