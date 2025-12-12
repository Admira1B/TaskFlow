using Microsoft.OpenApi;

namespace TaskFlow.Gateway {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen(options =>
                options.SwaggerDoc("v1", new OpenApiInfo {
                    Version = "v1",
                    Title = "TaskFlow Gateway",
                    Contact = new OpenApiContact {
                        Name = "Vlad Reizenbuk", Email = "vreizenbuk@mail.ru"
                    }
                })
            );

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.Run();
        }
    }
}
