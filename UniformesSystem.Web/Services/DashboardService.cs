using UniformesSystem.Web.Models;

namespace UniformesSystem.Web.Services
{
    public class DashboardService
    {
        private readonly EmployeeService _employeeService;
        private readonly ItemService _itemService;
        private readonly InventoryService _inventoryService;
        private readonly WarehouseMovementService _warehouseMovementService;

        public DashboardService(
            EmployeeService employeeService,
            ItemService itemService,
            InventoryService inventoryService,
            WarehouseMovementService warehouseMovementService)
        {
            _employeeService = employeeService;
            _itemService = itemService;
            _inventoryService = inventoryService;
            _warehouseMovementService = warehouseMovementService;
        }

        public async Task<DashboardData> GetDashboardDataAsync()
        {
            var dashboardData = new DashboardData();

            var employeesTask = _employeeService.GetEmployeesAsync();
            var itemsTask = _itemService.GetItemsAsync();
            var lowStockTask = _inventoryService.GetLowStockItemsAsync();
            var recentMovementsTask = _warehouseMovementService.GetRecentMovementsAsync(7);

            await Task.WhenAll(employeesTask, itemsTask, lowStockTask, recentMovementsTask);

            dashboardData.TotalEmployees = employeesTask.Result.Count;
            dashboardData.TotalItems = itemsTask.Result.Count;
            dashboardData.LowStockItems = lowStockTask.Result;
            
            dashboardData.RecentMovements = recentMovementsTask.Result
                .Where(m => m.MovementTypeId == 1) // Assuming 1 is the ID for issuance
                .ToList();
            
            dashboardData.TotalRecentTransactions = recentMovementsTask.Result.Count;

            return dashboardData;
        }
    }

    public class DashboardData
    {
        public int TotalEmployees { get; set; }
        public int TotalItems { get; set; }
        public int TotalRecentTransactions { get; set; }
        public List<InventoryDto> LowStockItems { get; set; } = new List<InventoryDto>();
        public List<WarehouseMovementDto> RecentMovements { get; set; } = new List<WarehouseMovementDto>();
    }
}
