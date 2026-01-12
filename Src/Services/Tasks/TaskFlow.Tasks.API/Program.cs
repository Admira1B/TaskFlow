using TaskFlow.Tasks.API.Extensions;

namespace TaskFlow.Tasks.API {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Adding Service dependencies
            builder.Services.AddTaskServiceDependencies(builder.Configuration);

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
