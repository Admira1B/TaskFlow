using Ocelot.Middleware;
using TaskFlow.Gateway.Composition;

namespace TaskFlow.Gateway {
    public class Program {
        public async static Task Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            builder.AddGatewayComposition();

            var app = builder.Build();

            app.UseAuthentication();
            app.UseAuthorization();

            await app.UseOcelot();

            app.Run();
        }
    }
}
