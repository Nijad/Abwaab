using Abwaab.Application.Interfaces;

namespace Abwaab.Server.Extensions
{
    public static class InitializerExtensions
    {
        public static async Task InitialiseDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var initializer = scope.ServiceProvider.GetRequiredService<IAppDbContextInitializer>();

            // Run migrations
            await initializer.InitializeAsync();

            // Seed data
            await initializer.SeedAsync();
        }
    }
}
