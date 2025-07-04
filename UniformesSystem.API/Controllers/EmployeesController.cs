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
    public class EmployeesController : ControllerBase
    {
        private readonly UniformesDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(UniformesDbContext context, IMapper mapper, ILogger<EmployeesController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployees()
        {
            var employees = await _context.Employees
                .Include(e => e.Group)
                .ThenInclude(g => g.EmployeeType)
                .ToListAsync();
                
            return Ok(_mapper.Map<IEnumerable<EmployeeDTO>>(employees));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployee(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Group)
                .ThenInclude(g => g.EmployeeType)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<EmployeeDTO>(employee));
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeDTO>> CreateEmployee(CreateEmployeeDTO employeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var groupExists = await _context.Groups.AnyAsync(g => g.GroupId == employeeDto.GroupId);
            if (!groupExists)
            {
                return BadRequest("The specified group does not exist.");
            }

            var employee = _mapper.Map<Employee>(employeeDto);
            
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // Reload with group info for response
            await _context.Entry(employee).Reference(e => e.Group).LoadAsync();
            await _context.Entry(employee.Group).Reference(g => g.EmployeeType).LoadAsync();

            return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeId }, _mapper.Map<EmployeeDTO>(employee));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, UpdateEmployeeDTO employeeDto)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            var groupExists = await _context.Groups.AnyAsync(g => g.GroupId == employeeDto.GroupId);
            if (!groupExists)
            {
                return BadRequest("The specified group does not exist.");
            }

            _mapper.Map(employeeDto, employee);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await EmployeeExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            // Check if the employee has any warehouse movements
            var hasWarehouseMovements = await _context.WarehouseMovements
                .AnyAsync(wm => wm.EmployeeId == id);
                
            if (hasWarehouseMovements)
            {
                return BadRequest("Cannot delete an employee with associated warehouse movements.");
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> EmployeeExists(int id)
        {
            return await _context.Employees.AnyAsync(e => e.EmployeeId == id);
        }

        [HttpGet("groups")]
        public async Task<ActionResult<IEnumerable<GroupDTO>>> GetGroups()
        {
            var groups = await _context.Groups
                .Include(g => g.EmployeeType)
                .ToListAsync();
                
            return Ok(_mapper.Map<IEnumerable<GroupDTO>>(groups));
        }

        [HttpGet("employeetypes")]
        public async Task<ActionResult<IEnumerable<EmployeeTypeDTO>>> GetEmployeeTypes()
        {
            var employeeTypes = await _context.EmployeeTypes.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<EmployeeTypeDTO>>(employeeTypes));
        }
    }
}
