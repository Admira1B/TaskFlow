using Microsoft.OpenApi;

namespace TaskFlow.Tasks.API {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen(options =>
                options.SwaggerDoc("v1", new OpenApiInfo {
                    Version = "v1",
                    Title = "TaskFlow Tasks Service",
                    Contact = new OpenApiContact {
                        Name = "Vlad Reizenbuk", Email = "vreizenbuk@mail.ru"
                    }
                })
            );

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.MapControllers();

            app.Run();
        }
    }
}
