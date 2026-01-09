using Microsoft.OpenApi;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Tasks.Application.Commands.Comment.CreateComment;
using TaskFlow.Tasks.Application.Mapping;
using TaskFlow.Tasks.Domain.Contracts;
using TaskFlow.Tasks.Infrastructure.SqlServer;
using TaskFlow.Tasks.Infrastructure.SqlServer.Repositories;

namespace TaskFlow.Tasks.API.Extensions {
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


            // Data Access
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IProjectMemberRepository, ProjectMemberRepository>();
            services.AddScoped<ITaskGroupRepository, TaskGroupRepository>();
            services.AddScoped<ITaskItemRepository, TaskItemRepository>();

            // MediatoR
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(CreateCommentCommandHandler).Assembly));

            // AutoMapper
            services.AddAutoMapper(typeof(TaskServiceMapperProfile).Assembly);

            return services;
        }
    }
}
