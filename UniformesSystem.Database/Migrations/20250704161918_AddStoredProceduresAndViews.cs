using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniformesSystem.Database.Migrations
{
    public partial class AddStoredProceduresAndViews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[sp_ReduceInventory]");
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[vw_WarehouseMovementsDetail]");
        }
    }
}
