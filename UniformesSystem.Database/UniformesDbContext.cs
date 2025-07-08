using Microsoft.EntityFrameworkCore;
using UniformesSystem.Database.Models;

namespace UniformesSystem.Database;

public class UniformesDbContext : DbContext
{
    public UniformesDbContext(DbContextOptions<UniformesDbContext> options) : base(options)
    {
    }
    
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<EmployeeType> EmployeeTypes { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<ItemType> ItemTypes { get; set; }
    public DbSet<ItemTypeEmployeeType> ItemTypeEmployeeTypes { get; set; }
    public DbSet<Size> Sizes { get; set; }
    public DbSet<Inventory> Inventory { get; set; }
    public DbSet<WarehouseMovement> WarehouseMovements { get; set; }
    public DbSet<WarehouseMovementDetail> WarehouseMovementDetails { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Inventory>()
            .HasKey(i => i.ItemId);
            
        modelBuilder.Entity<ItemTypeEmployeeType>()
            .HasKey(ite => new { ite.ItemTypeId, ite.EmployeeTypeId });
            
        modelBuilder.Entity<Employee>()
            .HasOne(e => e.Group)
            .WithMany(g => g.Employees)
            .HasForeignKey(e => e.GroupId);
            
        modelBuilder.Entity<Group>()
            .HasOne(g => g.EmployeeType)
            .WithMany(et => et.Groups)
            .HasForeignKey(g => g.EmployeeTypeId);
            
        modelBuilder.Entity<Item>()
            .HasOne(i => i.ItemType)
            .WithMany(it => it.Items)
            .HasForeignKey(i => i.ItemTypeId);
            
        modelBuilder.Entity<Item>()
            .HasOne(i => i.Size)
            .WithMany(s => s.Items)
            .HasForeignKey(i => i.SizeId);
            
        modelBuilder.Entity<Inventory>()
            .HasOne(i => i.Item)
            .WithMany(i => i.Inventories)
            .HasForeignKey(i => i.ItemId);
            
        modelBuilder.Entity<WarehouseMovement>()
            .HasOne(wm => wm.Employee)
            .WithMany(e => e.WarehouseMovements)
            .HasForeignKey(wm => wm.EmployeeId)
            .IsRequired(false);
            
        modelBuilder.Entity<WarehouseMovementDetail>()
            .HasOne(wmd => wmd.WarehouseMovement)
            .WithMany(wm => wm.Details)
            .HasForeignKey(wmd => wmd.WarehouseMovementId);
            
        modelBuilder.Entity<WarehouseMovementDetail>()
            .HasOne(wmd => wmd.Item)
            .WithMany(i => i.WarehouseMovementDetails)
            .HasForeignKey(wmd => wmd.ItemId);
            
        modelBuilder.Entity<ItemTypeEmployeeType>()
            .HasOne(ite => ite.ItemType)
            .WithMany(it => it.ItemTypeEmployeeTypes)
            .HasForeignKey(ite => ite.ItemTypeId);
            
        modelBuilder.Entity<ItemTypeEmployeeType>()
            .HasOne(ite => ite.EmployeeType)
            .WithMany(et => et.ItemTypeEmployeeTypes)
            .HasForeignKey(ite => ite.EmployeeTypeId);
            
        modelBuilder.Entity<Employee>().ToTable("Employees");
        modelBuilder.Entity<Employee>().Property(e => e.EmployeeId).HasColumnName("id_empleado");
        modelBuilder.Entity<Employee>().Property(e => e.Name).HasColumnName("nombre_empleado").IsRequired().HasMaxLength(100);
        modelBuilder.Entity<Employee>().Property(e => e.GroupId).HasColumnName("id_grupo");
        
        modelBuilder.Entity<Group>().ToTable("Groups");
        modelBuilder.Entity<Group>().Property(g => g.GroupId).HasColumnName("id_grupo");
        modelBuilder.Entity<Group>().Property(g => g.Name).HasColumnName("grupo").IsRequired().HasMaxLength(50);
        modelBuilder.Entity<Group>().Property(g => g.EmployeeTypeId).HasColumnName("id_tipo");
        
        modelBuilder.Entity<EmployeeType>().ToTable("EmployeeTypes");
        modelBuilder.Entity<EmployeeType>().Property(et => et.EmployeeTypeId).HasColumnName("id_tipo");
        modelBuilder.Entity<EmployeeType>().Property(et => et.Type).HasColumnName("tipo").IsRequired().HasMaxLength(50);
        
        modelBuilder.Entity<EmployeeType>().HasData(
            new EmployeeType { EmployeeTypeId = 1, Type = "Sindicalizados" },
            new EmployeeType { EmployeeTypeId = 2, Type = "Confianza" }
        );
        
        modelBuilder.Entity<Group>().HasData(
            new Group { GroupId = 1, Name = "A", EmployeeTypeId = 1 },
            new Group { GroupId = 2, Name = "B", EmployeeTypeId = 1 },
            new Group { GroupId = 3, Name = "C", EmployeeTypeId = 1 },
            new Group { GroupId = 4, Name = "D", EmployeeTypeId = 1 },
            new Group { GroupId = 5, Name = "E", EmployeeTypeId = 1 },
            new Group { GroupId = 6, Name = "Z", EmployeeTypeId = 2 }
        );
    }
}
