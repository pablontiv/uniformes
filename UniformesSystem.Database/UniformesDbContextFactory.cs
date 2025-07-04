using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace UniformesSystem.Database;

public class UniformesDbContextFactory : IDesignTimeDbContextFactory<UniformesDbContext>
{
    public UniformesDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("UNIFORMES_DB_CONNECTION") ?? 
            "Server=localhost;Database=UniformesDB;User=sa;Password=P@ssw0rd;TrustServerCertificate=True;";
        
        var optionsBuilder = new DbContextOptionsBuilder<UniformesDbContext>();
        optionsBuilder.UseSqlServer(connectionString);
        
        return new UniformesDbContext(optionsBuilder.Options);
    }
}
