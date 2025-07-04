using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace UniformesSystem.Database;

public static class MigrationStartupExtensions
{
    public static void MigrateDatabase(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<UniformesDbContext>();
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                // Log the exception if a logger is available
                Console.WriteLine($"An error occurred while migrating the database: {ex.Message}");
            }
        }
    }
}
