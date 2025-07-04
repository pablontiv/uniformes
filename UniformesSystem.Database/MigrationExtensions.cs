using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Reflection;

namespace UniformesSystem.Database;

public static class MigrationExtensions
{
    public static void ExecuteSqlScript(this MigrationBuilder migrationBuilder, string scriptName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"UniformesSystem.Database.Scripts.{scriptName}";
        
        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            throw new InvalidOperationException($"Could not find SQL script resource: {resourceName}");
        }

        using var reader = new StreamReader(stream);
        var sql = reader.ReadToEnd();
        
        migrationBuilder.Sql(sql);
    }
}
