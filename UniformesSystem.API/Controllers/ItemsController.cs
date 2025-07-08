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
    public class ItemsController : ControllerBase
    {
        private readonly UniformesDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ItemsController> _logger;

        public ItemsController(UniformesDbContext context, IMapper mapper, ILogger<ItemsController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResultDTO<ItemDTO>>> GetItems(
            int page = 1, 
            int pageSize = 50, 
            string? search = null,
            int? itemTypeId = null)
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 50;

            var query = _context.Items
                .Include(i => i.ItemType)
                .Include(i => i.Size)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(i => i.Name.Contains(search));
            }

            if (itemTypeId.HasValue)
            {
                query = query.Where(i => i.ItemTypeId == itemTypeId.Value);
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(i => i.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new PagedResultDTO<ItemDTO>
            {
                Items = _mapper.Map<IEnumerable<ItemDTO>>(items),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDTO>> GetItem(int id)
        {
            var item = await _context.Items
                .Include(i => i.ItemType)
                .Include(i => i.Size)
                .FirstOrDefaultAsync(i => i.ItemId == id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ItemDTO>(item));
        }

        [HttpPost]
        public async Task<ActionResult<ItemDTO>> CreateItem(CreateItemDTO itemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var itemTypeExists = await _context.ItemTypes.AnyAsync(it => it.ItemTypeId == itemDto.ItemTypeId);
            if (!itemTypeExists)
            {
                return BadRequest("The specified item type does not exist.");
            }

            var sizeExists = await _context.Sizes.AnyAsync(s => s.SizeId == itemDto.SizeId);
            if (!sizeExists)
            {
                return BadRequest("The specified size does not exist.");
            }

            var item = _mapper.Map<Item>(itemDto);
            
            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            var inventory = new Inventory
            {
                ItemId = item.ItemId,
                CurrentStock = 0,
                MinimumStock = 0
            };

            _context.Inventory.Add(inventory);
            await _context.SaveChangesAsync();

            await _context.Entry(item).Reference(i => i.ItemType).LoadAsync();
            await _context.Entry(item).Reference(i => i.Size).LoadAsync();

            return CreatedAtAction(nameof(GetItem), new { id = item.ItemId }, _mapper.Map<ItemDTO>(item));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, UpdateItemDTO itemDto)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            var itemTypeExists = await _context.ItemTypes.AnyAsync(it => it.ItemTypeId == itemDto.ItemTypeId);
            if (!itemTypeExists)
            {
                return BadRequest("The specified item type does not exist.");
            }

            var sizeExists = await _context.Sizes.AnyAsync(s => s.SizeId == itemDto.SizeId);
            if (!sizeExists)
            {
                return BadRequest("The specified size does not exist.");
            }

            _mapper.Map(itemDto, item);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ItemExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            var hasWarehouseMovements = await _context.WarehouseMovementDetails
                .AnyAsync(wmd => wmd.ItemId == id);
                
            if (hasWarehouseMovements)
            {
                return BadRequest("Cannot delete an item with associated warehouse movements.");
            }

            var inventory = await _context.Inventory.FindAsync(id);
            if (inventory != null)
            {
                _context.Inventory.Remove(inventory);
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> ItemExists(int id)
        {
            return await _context.Items.AnyAsync(i => i.ItemId == id);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<ItemTypeDTO>>> GetItemTypes()
        {
            var itemTypes = await _context.ItemTypes.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<ItemTypeDTO>>(itemTypes));
        }

        [HttpPost("types")]
        public async Task<ActionResult<ItemTypeDTO>> CreateItemType(CreateItemTypeDTO itemTypeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var itemType = _mapper.Map<ItemType>(itemTypeDto);
            
            _context.ItemTypes.Add(itemType);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetItemType), new { id = itemType.ItemTypeId }, _mapper.Map<ItemTypeDTO>(itemType));
        }

        [HttpGet("types/{id}")]
        public async Task<ActionResult<ItemTypeDTO>> GetItemType(int id)
        {
            var itemType = await _context.ItemTypes.FindAsync(id);

            if (itemType == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ItemTypeDTO>(itemType));
        }

        [HttpPut("types/{id}")]
        public async Task<IActionResult> UpdateItemType(int id, UpdateItemTypeDTO itemTypeDto)
        {
            var itemType = await _context.ItemTypes.FindAsync(id);
            if (itemType == null)
            {
                return NotFound();
            }

            _mapper.Map(itemTypeDto, itemType);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ItemTypeExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        private async Task<bool> ItemTypeExists(int id)
        {
            return await _context.ItemTypes.AnyAsync(i => i.ItemTypeId == id);
        }

        [HttpGet("sizes")]
        public async Task<ActionResult<IEnumerable<SizeDTO>>> GetSizes()
        {
            var sizes = await _context.Sizes.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<SizeDTO>>(sizes));
        }

        [HttpPost("sizes")]
        public async Task<ActionResult<SizeDTO>> CreateSize(CreateSizeDTO sizeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var size = _mapper.Map<Size>(sizeDto);
            
            _context.Sizes.Add(size);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSize), new { id = size.SizeId }, _mapper.Map<SizeDTO>(size));
        }

        [HttpGet("sizes/{id}")]
        public async Task<ActionResult<SizeDTO>> GetSize(int id)
        {
            var size = await _context.Sizes.FindAsync(id);

            if (size == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<SizeDTO>(size));
        }

        [HttpPut("sizes/{id}")]
        public async Task<IActionResult> UpdateSize(int id, UpdateSizeDTO sizeDto)
        {
            var size = await _context.Sizes.FindAsync(id);
            if (size == null)
            {
                return NotFound();
            }

            _mapper.Map(sizeDto, size);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await SizeExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        private async Task<bool> SizeExists(int id)
        {
            return await _context.Sizes.AnyAsync(s => s.SizeId == id);
        }
        
        [HttpGet("types/{itemTypeId}/employeetypes")]
        public async Task<ActionResult<IEnumerable<ItemTypeEmployeeTypeDTO>>> GetItemTypeEmployeeTypes(int itemTypeId)
        {
            var itemTypeExists = await _context.ItemTypes.AnyAsync(it => it.ItemTypeId == itemTypeId);
            if (!itemTypeExists)
            {
                return NotFound("Item type not found.");
            }
            
            var itemTypeEmployeeTypes = await _context.ItemTypeEmployeeTypes
                .Include(ite => ite.ItemType)
                .Include(ite => ite.EmployeeType)
                .Where(ite => ite.ItemTypeId == itemTypeId)
                .ToListAsync();
                
            return Ok(_mapper.Map<IEnumerable<ItemTypeEmployeeTypeDTO>>(itemTypeEmployeeTypes));
        }
        
        [HttpPost("types/{itemTypeId}/employeetypes")]
        public async Task<ActionResult<ItemTypeEmployeeTypeDTO>> AssignItemTypeToEmployeeType(int itemTypeId, CreateItemTypeEmployeeTypeDTO dto)
        {
            if (itemTypeId != dto.ItemTypeId)
            {
                return BadRequest("Item type ID in URL must match body.");
            }
            
            var itemTypeExists = await _context.ItemTypes.AnyAsync(it => it.ItemTypeId == itemTypeId);
            if (!itemTypeExists)
            {
                return NotFound("Item type not found.");
            }
            
            var employeeTypeExists = await _context.EmployeeTypes.AnyAsync(et => et.EmployeeTypeId == dto.EmployeeTypeId);
            if (!employeeTypeExists)
            {
                return NotFound("Employee type not found.");
            }
            
            var exists = await _context.ItemTypeEmployeeTypes
                .AnyAsync(ite => ite.ItemTypeId == dto.ItemTypeId && ite.EmployeeTypeId == dto.EmployeeTypeId);
                
            if (exists)
            {
                return BadRequest("This assignment already exists.");
            }
            
            var itemTypeEmployeeType = _mapper.Map<ItemTypeEmployeeType>(dto);
            _context.ItemTypeEmployeeTypes.Add(itemTypeEmployeeType);
            await _context.SaveChangesAsync();
            
            await _context.Entry(itemTypeEmployeeType).Reference(ite => ite.ItemType).LoadAsync();
            await _context.Entry(itemTypeEmployeeType).Reference(ite => ite.EmployeeType).LoadAsync();
            
            return Ok(_mapper.Map<ItemTypeEmployeeTypeDTO>(itemTypeEmployeeType));
        }
        
        [HttpDelete("types/{itemTypeId}/employeetypes/{employeeTypeId}")]
        public async Task<IActionResult> RemoveItemTypeFromEmployeeType(int itemTypeId, int employeeTypeId)
        {
            var assignment = await _context.ItemTypeEmployeeTypes
                .FirstOrDefaultAsync(ite => ite.ItemTypeId == itemTypeId && ite.EmployeeTypeId == employeeTypeId);
                
            if (assignment == null)
            {
                return NotFound("Assignment not found.");
            }
            
            _context.ItemTypeEmployeeTypes.Remove(assignment);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
    }
}
