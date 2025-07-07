using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniformesSystem.API.Models.DTOs;
using UniformesSystem.Database;

namespace UniformesSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly UniformesDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(UniformesDbContext context, IMapper mapper, ILogger<ReportsController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("inventory")]
        public async Task<ActionResult<IEnumerable<InventoryReportDTO>>> GetInventoryReport()
        {
            var inventoryReport = await _context.Inventory
                .Include(i => i.Item)
                .ThenInclude(i => i.Size)
                .Include(i => i.Item.ItemType)
                .Select(i => new InventoryReportDTO
                {
                    ItemId = i.ItemId,
                    ItemName = i.Item.Name,
                    ItemType = i.Item.ItemType.Name,
                    SizeName = i.Item.Size.Value,
                    CurrentStock = i.CurrentStock,
                    MinimumStock = i.MinimumStock,
                    StockStatus = i.CurrentStock <= i.MinimumStock ? "Low Stock" : "Adequate",
                    LastUpdated = DateTime.Now
                })
                .OrderBy(i => i.ItemType)
                .ThenBy(i => i.ItemName)
                .ThenBy(i => i.SizeName)
                .ToListAsync();

            return Ok(inventoryReport);
        }

        [HttpGet("transactions")]
        public async Task<ActionResult<IEnumerable<TransactionReportDTO>>> GetTransactionReport(
            DateTime? startDate = null,
            DateTime? endDate = null,
            int? employeeId = null,
            int? itemId = null)
        {
            var query = _context.WarehouseMovements
                .Include(wm => wm.Employee)
                .Include(wm => wm.Details)
                .ThenInclude(d => d.Item)
                .ThenInclude(i => i.Size)
                .AsQueryable();

            if (startDate.HasValue)
                query = query.Where(wm => wm.Date >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(wm => wm.Date <= endDate.Value);

            if (employeeId.HasValue)
                query = query.Where(wm => wm.EmployeeId == employeeId);

            if (itemId.HasValue)
                query = query.Where(wm => wm.Details.Any(d => d.ItemId == itemId));

            var transactions = await query
                .OrderByDescending(wm => wm.Date)
                .ToListAsync();

            var transactionReport = transactions.SelectMany(wm => wm.Details.Select(d => new TransactionReportDTO
            {
                TransactionId = wm.WarehouseMovementId,
                Date = wm.Date,
                MovementType = wm.MovementType.ToString(),
                EmployeeName = wm.Employee?.Name ?? "N/A",
                ItemName = d.Item.Name,
                SizeName = d.Item.Size.Value,
                Quantity = d.Quantity,
                Notes = wm.Notes
            }));

            return Ok(transactionReport);
        }

        [HttpGet("employee-uniforms")]
        public async Task<ActionResult<IEnumerable<EmployeeUniformReportDTO>>> GetEmployeeUniformReport(int? employeeId = null)
        {
            var query = _context.WarehouseMovements
                .Include(wm => wm.Employee)
                .ThenInclude(e => e.Group)
                .Include(wm => wm.Details)
                .ThenInclude(d => d.Item)
                .ThenInclude(i => i.Size)
                .Where(wm => wm.MovementType == Database.Models.MovementType.EmployeeIssuance)
                .AsQueryable();

            if (employeeId.HasValue)
                query = query.Where(wm => wm.EmployeeId == employeeId);

            var issuanceMovements = await query
                .OrderByDescending(wm => wm.Date)
                .ToListAsync();

            var employeeUniformReport = issuanceMovements
                .SelectMany(wm => wm.Details.Select(d => new EmployeeUniformReportDTO
                {
                    EmployeeId = wm.Employee.EmployeeId,
                    EmployeeName = wm.Employee.Name,
                    GroupName = wm.Employee.Group.Name,
                    ItemName = d.Item.Name,
                    SizeName = d.Item.Size.Value,
                    QuantityIssued = d.Quantity,
                    IssuanceDate = wm.Date,
                    Notes = wm.Notes
                }))
                .OrderBy(r => r.EmployeeName)
                .ThenByDescending(r => r.IssuanceDate)
                .ToList();

            return Ok(employeeUniformReport);
        }

        [HttpGet("low-stock")]
        public async Task<ActionResult<IEnumerable<LowStockReportDTO>>> GetLowStockReport()
        {
            var lowStockItems = await _context.Inventory
                .Include(i => i.Item)
                .ThenInclude(i => i.Size)
                .Include(i => i.Item.ItemType)
                .Where(i => i.CurrentStock <= i.MinimumStock)
                .Select(i => new LowStockReportDTO
                {
                    ItemId = i.ItemId,
                    ItemName = i.Item.Name,
                    ItemType = i.Item.ItemType.Name,
                    SizeName = i.Item.Size.Value,
                    CurrentStock = i.CurrentStock,
                    MinimumStock = i.MinimumStock,
                    StockDeficit = i.MinimumStock - i.CurrentStock,
                    LastUpdated = DateTime.Now
                })
                .OrderBy(i => i.StockDeficit)
                .ThenBy(i => i.ItemType)
                .ThenBy(i => i.ItemName)
                .ToListAsync();

            return Ok(lowStockItems);
        }

        [HttpGet("summary")]
        public async Task<ActionResult<ReportSummaryDTO>> GetReportSummary()
        {
            var totalItems = await _context.Items.CountAsync();
            var totalInventoryValue = await _context.Inventory.SumAsync(i => i.CurrentStock);
            var lowStockCount = await _context.Inventory.CountAsync(i => i.CurrentStock <= i.MinimumStock);
            var totalEmployees = await _context.Employees.CountAsync();
            var totalMovements = await _context.WarehouseMovements.CountAsync();
            var recentMovements = await _context.WarehouseMovements
                .Where(wm => wm.Date >= DateTime.Now.AddDays(-30))
                .CountAsync();

            var summary = new ReportSummaryDTO
            {
                TotalItems = totalItems,
                TotalInventoryQuantity = totalInventoryValue,
                LowStockItemsCount = lowStockCount,
                TotalEmployees = totalEmployees,
                TotalMovements = totalMovements,
                RecentMovements = recentMovements,
                GeneratedAt = DateTime.Now
            };

            return Ok(summary);
        }
    }
}