using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UniformesSystem.Database;

namespace UniformesSystem.API
{
    public class DbChecker
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine($"Connection string: {connectionString}");

            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<UniformesDbContext>();
                optionsBuilder.UseSqlServer(connectionString);

                using (var context = new UniformesDbContext(optionsBuilder.Options))
                {
                    Console.WriteLine("Connected to database successfully!");
                    
                    var tables = context.Model.GetEntityTypes().Select(t => t.GetTableName()).ToList();
                    Console.WriteLine($"Tables in database ({tables.Count}):");
                    foreach (var table in tables)
                    {
                        Console.WriteLine($"- {table}");
                    }
                    
                    var employeesTable = tables.FirstOrDefault(t => t == "Employees");
                    Console.WriteLine($"Employees table exists: {employeesTable != null}");
                    
                    var groupsTable = tables.FirstOrDefault(t => t == "Groups");
                    Console.WriteLine($"Groups table exists: {groupsTable != null}");
                    
                    var itemsTable = tables.FirstOrDefault(t => t == "Items");
                    Console.WriteLine($"Items table exists: {itemsTable != null}");
                    
                    var inventoryTable = tables.FirstOrDefault(t => t == "Inventory");
                    Console.WriteLine($"Inventory table exists: {inventoryTable != null}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to database: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
        }
    }
}
