using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniformesSystem.Database.Migrations
{
    public partial class PerformanceIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_WarehouseMovements_Date_MovementType')
                    CREATE NONCLUSTERED INDEX [IX_WarehouseMovements_Date_MovementType] 
                    ON [WarehouseMovements] ([Date], [MovementType]);
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_WarehouseMovements_Date_EmployeeId')
                    CREATE NONCLUSTERED INDEX [IX_WarehouseMovements_Date_EmployeeId] 
                    ON [WarehouseMovements] ([Date], [EmployeeId]);
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Inventory_CurrentStock_MinimumStock')
                    CREATE NONCLUSTERED INDEX [IX_Inventory_CurrentStock_MinimumStock] 
                    ON [Inventory] ([CurrentStock], [MinimumStock]);
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Items_Name')
                    CREATE NONCLUSTERED INDEX [IX_Items_Name] 
                    ON [Items] ([Name]);
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Employees_Name')
                    CREATE NONCLUSTERED INDEX [IX_Employees_Name] 
                    ON [Employees] ([nombre_empleado]);
            ");

            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_WarehouseMovementDetails_ItemId')
                    CREATE NONCLUSTERED INDEX [IX_WarehouseMovementDetails_ItemId] 
                    ON [WarehouseMovementDetails] ([ItemId]);
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_WarehouseMovements_Date_MovementType')
                    DROP INDEX [IX_WarehouseMovements_Date_MovementType] ON [WarehouseMovements];
            ");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_WarehouseMovements_Date_EmployeeId')
                    DROP INDEX [IX_WarehouseMovements_Date_EmployeeId] ON [WarehouseMovements];
            ");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Inventory_CurrentStock_MinimumStock')
                    DROP INDEX [IX_Inventory_CurrentStock_MinimumStock] ON [Inventory];
            ");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Items_Name')
                    DROP INDEX [IX_Items_Name] ON [Items];
            ");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Employees_Name')
                    DROP INDEX [IX_Employees_Name] ON [Employees];
            ");

            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_WarehouseMovementDetails_ItemId')
                    DROP INDEX [IX_WarehouseMovementDetails_ItemId] ON [WarehouseMovementDetails];
            ");
        }
    }
}