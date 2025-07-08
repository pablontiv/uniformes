using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniformesSystem.API.Controllers;
using UniformesSystem.API.Models.DTOs;
using UniformesSystem.Database;
using UniformesSystem.Database.Models;
using Xunit;

namespace UniformesSystem.API.Tests
{
    public class EmployeesControllerTests
    {
        private readonly Mock<ILogger<EmployeesController>> _loggerMock;
        private readonly IMapper _mapper;
        private readonly UniformesDbContext _context;

        public EmployeesControllerTests()
        {
            var options = new DbContextOptionsBuilder<UniformesDbContext>()
                .UseInMemoryDatabase(databaseName: "TestEmployeeDb")
                .Options;
            
            _context = new UniformesDbContext(options);
            
            InitializeTestData();
            
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeDTO>()
                    .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.Group.Name))
                    .ForMember(dest => dest.EmployeeType, opt => opt.MapFrom(src => src.Group.EmployeeType.Type));
                cfg.CreateMap<CreateEmployeeDTO, Employee>();
                cfg.CreateMap<UpdateEmployeeDTO, Employee>();
            });
            
            _mapper = mapperConfig.CreateMapper();
            _loggerMock = new Mock<ILogger<EmployeesController>>();
        }
        
        private void InitializeTestData()
        {
            _context.Database.EnsureDeleted();
            
            var unionized = new EmployeeType { EmployeeTypeId = 1, Type = "Sindicalizados" };
            var administrative = new EmployeeType { EmployeeTypeId = 2, Type = "Confianza" };
            _context.EmployeeTypes.Add(unionized);
            _context.EmployeeTypes.Add(administrative);
            
            var groupA = new Group { GroupId = 1, Name = "A", EmployeeTypeId = 1, EmployeeType = unionized };
            var groupZ = new Group { GroupId = 6, Name = "Z", EmployeeTypeId = 2, EmployeeType = administrative };
            _context.Groups.Add(groupA);
            _context.Groups.Add(groupZ);
            
            var employee1 = new Employee { EmployeeId = 1, Name = "John Doe", GroupId = 1, Group = groupA };
            var employee2 = new Employee { EmployeeId = 2, Name = "Jane Smith", GroupId = 6, Group = groupZ };
            _context.Employees.Add(employee1);
            _context.Employees.Add(employee2);
            
            _context.SaveChanges();
        }
        
        [Fact]
        public async Task GetEmployees_ReturnsAllEmployees()
        {
            var controller = new EmployeesController(_context, _mapper, _loggerMock.Object);
            
            var result = await controller.GetEmployees();
            
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var pagedResult = Assert.IsAssignableFrom<PagedResultDTO<EmployeeDTO>>(okResult.Value);
            Assert.Equal(2, pagedResult.Items.Count());
            Assert.Equal(2, pagedResult.TotalCount);
        }
        
        [Fact]
        public async Task GetEmployee_WithValidId_ReturnsEmployee()
        {
            var controller = new EmployeesController(_context, _mapper, _loggerMock.Object);
            
            var result = await controller.GetEmployee(1);
            
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var employee = Assert.IsType<EmployeeDTO>(okResult.Value);
            Assert.Equal("John Doe", employee.Name);
            Assert.Equal("A", employee.GroupName);
            Assert.Equal("Sindicalizados", employee.EmployeeType);
        }
        
        [Fact]
        public async Task CreateEmployee_WithValidData_ReturnsCreatedEmployee()
        {
            // Arrange
            var controller = new EmployeesController(_context, _mapper, _loggerMock.Object);
            var newEmployee = new CreateEmployeeDTO
            {
                Name = "New Employee",
                GroupId = 1
            };
            
            var result = await controller.CreateEmployee(newEmployee);
            
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var employee = Assert.IsType<EmployeeDTO>(createdResult.Value);
            Assert.Equal("New Employee", employee.Name);
            Assert.Equal(1, employee.GroupId);
            
            Assert.Equal(3, _context.Employees.Count());
        }
    }
}
