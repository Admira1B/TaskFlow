using TaskFlow.Identity.API.Extensions;

namespace TaskFlow.Identity.API {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Adding Service dependencies
            builder.Services.AddIdentityServiceDependencies(builder.Configuration);

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
