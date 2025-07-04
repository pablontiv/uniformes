using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniformesSystem.API.Models.DTOs;
using UniformesSystem.Database;
using UniformesSystem.Database.Models;

namespace UniformesSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WarehouseMovementsController : ControllerBase
    {
        private readonly UniformesDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<WarehouseMovementsController> _logger;

        public WarehouseMovementsController(UniformesDbContext context, IMapper mapper, ILogger<WarehouseMovementsController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WarehouseMovementDTO>>> GetWarehouseMovements()
        {
            var movements = await _context.WarehouseMovements
                .Include(wm => wm.Employee)
                .Include(wm => wm.Details)
                .ThenInclude(d => d.Item)
                .ThenInclude(i => i.Size)
                .OrderByDescending(wm => wm.Date)
                .ToListAsync();
                
            return Ok(_mapper.Map<IEnumerable<WarehouseMovementDTO>>(movements));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WarehouseMovementDTO>> GetWarehouseMovement(int id)
        {
            var movement = await _context.WarehouseMovements
                .Include(wm => wm.Employee)
                .Include(wm => wm.Details)
                .ThenInclude(d => d.Item)
                .ThenInclude(i => i.Size)
                .FirstOrDefaultAsync(wm => wm.WarehouseMovementId == id);

            if (movement == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<WarehouseMovementDTO>(movement));
        }

        [HttpPost]
        public async Task<ActionResult<WarehouseMovementDTO>> CreateWarehouseMovement(CreateWarehouseMovementDTO movementDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            // For issuance movements (reduction), employee is required
            if (movementDto.MovementType == MovementType.EmployeeIssuance && !movementDto.EmployeeId.HasValue)
            {
                return BadRequest("Employee ID is required for issuance movements.");
            }
            
            // If employee ID is provided, verify it exists
            if (movementDto.EmployeeId.HasValue)
            {
                var employeeExists = await _context.Employees.AnyAsync(e => e.EmployeeId == movementDto.EmployeeId);
                if (!employeeExists)
                {
                    return BadRequest("The specified employee does not exist.");
                }
                
                // For issuance movements, verify item types are allowed for the employee's type
                if (movementDto.MovementType == MovementType.EmployeeIssuance)
                {
                    var employee = await _context.Employees
                        .Include(e => e.Group)
                        .FirstOrDefaultAsync(e => e.EmployeeId == movementDto.EmployeeId);
                        
                    if (employee == null)
                    {
                        return BadRequest("The specified employee does not exist.");
                    }
                    
                    foreach (var detail in movementDto.Details)
                    {
                        var item = await _context.Items
                            .Include(i => i.ItemType)
                            .FirstOrDefaultAsync(i => i.ItemId == detail.ItemId);
                            
                        if (item == null)
                        {
                            return BadRequest($"Item with ID {detail.ItemId} does not exist.");
                        }
                        
                        // Check if this item type is allowed for this employee's type
                        var isAllowed = await _context.ItemTypeEmployeeTypes
                            .AnyAsync(ite => ite.ItemTypeId == item.ItemTypeId && 
                                             ite.EmployeeTypeId == employee.Group.EmployeeTypeId);
                                             
                        if (!isAllowed)
                        {
                            return BadRequest($"Item {item.Name} cannot be issued to this employee type.");
                        }
                    }
                }
            }
            
            // Verify all items exist and have sufficient stock for issuance
            foreach (var detail in movementDto.Details)
            {
                var item = await _context.Items.FindAsync(detail.ItemId);
                if (item == null)
                {
                    return BadRequest($"Item with ID {detail.ItemId} does not exist.");
                }
                
                // For issuance, check if there's enough stock
                if (movementDto.MovementType == MovementType.EmployeeIssuance)
                {
                    var inventory = await _context.Inventory.FindAsync(detail.ItemId);
                    if (inventory == null || inventory.CurrentStock < detail.Quantity)
                    {
                        return BadRequest($"Insufficient stock for item {item.Name}.");
                    }
                }
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                var movement = _mapper.Map<WarehouseMovement>(movementDto);
                
                _context.WarehouseMovements.Add(movement);
                await _context.SaveChangesAsync();
                
                // Update inventory levels
                foreach (var detail in movement.Details)
                {
                    var inventory = await _context.Inventory.FindAsync(detail.ItemId);
                    if (inventory != null)
                    {
                        if (movement.MovementType == MovementType.PurchaseReceipt || 
                            movement.MovementType == MovementType.EmployeeReturn ||
                            movement.MovementType == MovementType.InventoryAdjustment)
                        {
                            inventory.CurrentStock += detail.Quantity;
                        }
                        else if (movement.MovementType == MovementType.EmployeeIssuance)
                        {
                            inventory.CurrentStock -= detail.Quantity;
                        }
                    }
                }
                
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                
                // Reload with related data for response
                await _context.Entry(movement).Reference(wm => wm.Employee).LoadAsync();
                await _context.Entry(movement).Collection(wm => wm.Details).LoadAsync();
                foreach (var detail in movement.Details)
                {
                    await _context.Entry(detail).Reference(d => d.Item).LoadAsync();
                    await _context.Entry(detail.Item).Reference(i => i.Size).LoadAsync();
                }
                
                return CreatedAtAction(nameof(GetWarehouseMovement), 
                    new { id = movement.WarehouseMovementId }, 
                    _mapper.Map<WarehouseMovementDTO>(movement));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error creating warehouse movement");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<IEnumerable<WarehouseMovementDTO>>> GetMovementsByEmployee(int employeeId)
        {
            var employeeExists = await _context.Employees.AnyAsync(e => e.EmployeeId == employeeId);
            if (!employeeExists)
            {
                return NotFound("Employee not found.");
            }
            
            var movements = await _context.WarehouseMovements
                .Include(wm => wm.Employee)
                .Include(wm => wm.Details)
                .ThenInclude(d => d.Item)
                .ThenInclude(i => i.Size)
                .Where(wm => wm.EmployeeId == employeeId)
                .OrderByDescending(wm => wm.Date)
                .ToListAsync();
                
            return Ok(_mapper.Map<IEnumerable<WarehouseMovementDTO>>(movements));
        }
        
        [HttpGet("item/{itemId}")]
        public async Task<ActionResult<IEnumerable<WarehouseMovementDTO>>> GetMovementsByItem(int itemId)
        {
            var itemExists = await _context.Items.AnyAsync(i => i.ItemId == itemId);
            if (!itemExists)
            {
                return NotFound("Item not found.");
            }
            
            var movements = await _context.WarehouseMovements
                .Include(wm => wm.Employee)
                .Include(wm => wm.Details.Where(d => d.ItemId == itemId))
                .ThenInclude(d => d.Item)
                .ThenInclude(i => i.Size)
                .Where(wm => wm.Details.Any(d => d.ItemId == itemId))
                .OrderByDescending(wm => wm.Date)
                .ToListAsync();
                
            return Ok(_mapper.Map<IEnumerable<WarehouseMovementDTO>>(movements));
        }
    }
}
