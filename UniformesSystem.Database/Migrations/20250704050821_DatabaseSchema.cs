using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UniformesSystem.Database.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeTypes",
                columns: table => new
                {
                    id_tipo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tipo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTypes", x => x.id_tipo);
                });

            migrationBuilder.CreateTable(
                name: "ItemTypes",
                columns: table => new
                {
                    ItemTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsUniversal = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTypes", x => x.ItemTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Sizes",
                columns: table => new
                {
                    SizeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    System = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sizes", x => x.SizeId);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    id_grupo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    grupo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    id_tipo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.id_grupo);
                    table.ForeignKey(
                        name: "FK_Groups_EmployeeTypes_id_tipo",
                        column: x => x.id_tipo,
                        principalTable: "EmployeeTypes",
                        principalColumn: "id_tipo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemTypeEmployeeTypes",
                columns: table => new
                {
                    ItemTypeId = table.Column<int>(type: "int", nullable: false),
                    EmployeeTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTypeEmployeeTypes", x => new { x.ItemTypeId, x.EmployeeTypeId });
                    table.ForeignKey(
                        name: "FK_ItemTypeEmployeeTypes_EmployeeTypes_EmployeeTypeId",
                        column: x => x.EmployeeTypeId,
                        principalTable: "EmployeeTypes",
                        principalColumn: "id_tipo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemTypeEmployeeTypes_ItemTypes_ItemTypeId",
                        column: x => x.ItemTypeId,
                        principalTable: "ItemTypes",
                        principalColumn: "ItemTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemTypeId = table.Column<int>(type: "int", nullable: false),
                    SizeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_Items_ItemTypes_ItemTypeId",
                        column: x => x.ItemTypeId,
                        principalTable: "ItemTypes",
                        principalColumn: "ItemTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Items_Sizes_SizeId",
                        column: x => x.SizeId,
                        principalTable: "Sizes",
                        principalColumn: "SizeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    id_empleado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_empleado = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    id_grupo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.id_empleado);
                    table.ForeignKey(
                        name: "FK_Employees_Groups_id_grupo",
                        column: x => x.id_grupo,
                        principalTable: "Groups",
                        principalColumn: "id_grupo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inventory",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    CurrentStock = table.Column<int>(type: "int", nullable: false),
                    MinimumStock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventory", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_Inventory_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseMovements",
                columns: table => new
                {
                    WarehouseMovementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MovementType = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseMovements", x => x.WarehouseMovementId);
                    table.ForeignKey(
                        name: "FK_WarehouseMovements_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "id_empleado");
                });

            migrationBuilder.CreateTable(
                name: "WarehouseMovementDetails",
                columns: table => new
                {
                    WarehouseMovementDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WarehouseMovementId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseMovementDetails", x => x.WarehouseMovementDetailId);
                    table.ForeignKey(
                        name: "FK_WarehouseMovementDetails_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarehouseMovementDetails_WarehouseMovements_WarehouseMovementId",
                        column: x => x.WarehouseMovementId,
                        principalTable: "WarehouseMovements",
                        principalColumn: "WarehouseMovementId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "EmployeeTypes",
                columns: new[] { "id_tipo", "tipo" },
                values: new object[,]
                {
                    { 1, "Sindicalizados" },
                    { 2, "Confianza" }
                });

            migrationBuilder.InsertData(
                table: "Groups",
                columns: new[] { "id_grupo", "id_tipo", "grupo" },
                values: new object[,]
                {
                    { 1, 1, "A" },
                    { 2, 1, "B" },
                    { 3, 1, "C" },
                    { 4, 1, "D" },
                    { 5, 1, "E" },
                    { 6, 2, "Z" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_id_grupo",
                table: "Employees",
                column: "id_grupo");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_id_tipo",
                table: "Groups",
                column: "id_tipo");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemTypeId",
                table: "Items",
                column: "ItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_SizeId",
                table: "Items",
                column: "SizeId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTypeEmployeeTypes_EmployeeTypeId",
                table: "ItemTypeEmployeeTypes",
                column: "EmployeeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseMovementDetails_ItemId",
                table: "WarehouseMovementDetails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseMovementDetails_WarehouseMovementId",
                table: "WarehouseMovementDetails",
                column: "WarehouseMovementId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseMovements_EmployeeId",
                table: "WarehouseMovements",
                column: "EmployeeId");
                
            // Create stored procedure for inventory reduction
            migrationBuilder.Sql(@"
-- Stored procedure for inventory reduction when issuing items to employees
CREATE OR ALTER PROCEDURE [dbo].[sp_ReduceInventory]
    @ItemId INT,
    @Quantity INT,
    @Success BIT OUTPUT,
    @ErrorMessage NVARCHAR(255) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @CurrentStock INT;
        SELECT @CurrentStock = CurrentStock FROM Inventory WHERE ItemId = @ItemId;

        IF @CurrentStock IS NULL
        BEGIN
            SET @Success = 0;
            SET @ErrorMessage = 'Item not found in inventory';
            ROLLBACK TRANSACTION;
            RETURN;
        END

        IF @CurrentStock < @Quantity
        BEGIN
            SET @Success = 0;
            SET @ErrorMessage = 'Insufficient inventory';
            ROLLBACK TRANSACTION;
            RETURN;
        END

        UPDATE Inventory
        SET CurrentStock = CurrentStock - @Quantity
        WHERE ItemId = @ItemId;

        SET @Success = 1;
        SET @ErrorMessage = NULL;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        SET @Success = 0;
        SET @ErrorMessage = ERROR_MESSAGE();
    END CATCH
END");

            // Create view for warehouse movements with associated employee information
            migrationBuilder.Sql(@"
CREATE OR ALTER VIEW [dbo].[vw_WarehouseMovementsDetail] AS
SELECT 
    wm.WarehouseMovementId,
    wm.Date,
    wm.MovementType,
    wm.Notes,
    e.id_empleado AS EmployeeId,
    e.nombre_empleado AS EmployeeName,
    g.grupo AS GroupName,
    et.tipo AS EmployeeType,
    wmd.WarehouseMovementDetailId,
    wmd.ItemId,
    i.Name AS ItemName,
    i.Description AS ItemDescription,
    s.Value AS SizeValue,
    s.System AS SizeSystem,
    wmd.Quantity
FROM 
    WarehouseMovements wm
    LEFT JOIN Employees e ON wm.EmployeeId = e.id_empleado
    LEFT JOIN Groups g ON e.id_grupo = g.id_grupo
    LEFT JOIN EmployeeTypes et ON g.id_tipo = et.id_tipo
    INNER JOIN WarehouseMovementDetails wmd ON wm.WarehouseMovementId = wmd.WarehouseMovementId
    INNER JOIN Items i ON wmd.ItemId = i.ItemId
    INNER JOIN Sizes s ON i.SizeId = s.SizeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inventory");

            migrationBuilder.DropTable(
                name: "ItemTypeEmployeeTypes");

            migrationBuilder.DropTable(
                name: "WarehouseMovementDetails");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "WarehouseMovements");

            migrationBuilder.DropTable(
                name: "ItemTypes");

            migrationBuilder.DropTable(
                name: "Sizes");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "EmployeeTypes");
        }
    }
}
