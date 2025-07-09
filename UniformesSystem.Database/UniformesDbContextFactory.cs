using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace UniformesSystem.Database;

public class UniformesDbContextFactory : IDesignTimeDbContextFactory<UniformesDbContext>
{
    public UniformesDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("UNIFORMES_DB_CONNECTION") ?? 
            "Server=(localdb)\\mssqllocaldb;Database=UniformesDB;Trusted_Connection=True;TrustServerCertificate=True;";
        
        var optionsBuilder = new DbContextOptionsBuilder<UniformesDbContext>();
        optionsBuilder.UseSqlServer(connectionString);
        
        return new UniformesDbContext(optionsBuilder.Options);
    }
}
