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
    public class InventoryController : ControllerBase
    {
        private readonly UniformesDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(UniformesDbContext context, IMapper mapper, ILogger<InventoryController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryDTO>>> GetInventory()
        {
            var inventoryItems = await _context.Inventory
                .Include(i => i.Item)
                .ThenInclude(i => i.Size)
                .ToListAsync();
                
            return Ok(_mapper.Map<IEnumerable<InventoryDTO>>(inventoryItems));
        }
        
        [HttpGet("lowstock")]
        public async Task<ActionResult<IEnumerable<InventoryDTO>>> GetLowStockItems()
        {
            var lowStockItems = await _context.Inventory
                .Include(i => i.Item)
                .ThenInclude(i => i.Size)
                .Where(i => i.CurrentStock <= i.MinimumStock)
                .ToListAsync();
                
            return Ok(_mapper.Map<IEnumerable<InventoryDTO>>(lowStockItems));
        }

        [HttpGet("{itemId}")]
        public async Task<ActionResult<InventoryDTO>> GetInventoryItem(int itemId)
        {
            var inventoryItem = await _context.Inventory
                .Include(i => i.Item)
                .ThenInclude(i => i.Size)
                .FirstOrDefaultAsync(i => i.ItemId == itemId);

            if (inventoryItem == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<InventoryDTO>(inventoryItem));
        }

        [HttpPut("{itemId}")]
        public async Task<IActionResult> UpdateMinimumStock(int itemId, UpdateInventoryDTO inventoryDto)
        {
            var inventoryItem = await _context.Inventory.FindAsync(itemId);
            if (inventoryItem == null)
            {
                return NotFound();
            }

            inventoryItem.MinimumStock = inventoryDto.MinimumStock;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await InventoryItemExists(itemId))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        private async Task<bool> InventoryItemExists(int itemId)
        {
            return await _context.Inventory.AnyAsync(i => i.ItemId == itemId);
        }
    }
}
