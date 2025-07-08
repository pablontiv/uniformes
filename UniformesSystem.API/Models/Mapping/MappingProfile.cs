using AutoMapper;
using UniformesSystem.API.Models.DTOs;
using UniformesSystem.Database.Models;

namespace UniformesSystem.API.Models.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeDTO>()
                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.Group.Name))
                .ForMember(dest => dest.EmployeeType, opt => opt.MapFrom(src => src.Group.EmployeeType.Type));
            CreateMap<CreateEmployeeDTO, Employee>();
            CreateMap<UpdateEmployeeDTO, Employee>();

            CreateMap<Group, GroupDTO>()
                .ForMember(dest => dest.EmployeeTypeName, opt => opt.MapFrom(src => src.EmployeeType.Type));

            CreateMap<EmployeeType, EmployeeTypeDTO>();

            CreateMap<ItemType, ItemTypeDTO>();
            CreateMap<CreateItemTypeDTO, ItemType>();
            CreateMap<UpdateItemTypeDTO, ItemType>();

            CreateMap<Size, SizeDTO>()
                .ForMember(dest => dest.SizeName, opt => opt.MapFrom(src => src.Value))
                .ForMember(dest => dest.SizeType, opt => opt.MapFrom(src => src.System.ToString()));
            CreateMap<CreateSizeDTO, Size>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.SizeName))
                .ForMember(dest => dest.System, opt => opt.MapFrom(src => 
                    Enum.Parse<SizeSystem>(src.SizeType)));
            CreateMap<UpdateSizeDTO, Size>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.SizeName))
                .ForMember(dest => dest.System, opt => opt.MapFrom(src => 
                    Enum.Parse<SizeSystem>(src.SizeType)));

            CreateMap<Item, ItemDTO>()
                .ForMember(dest => dest.ItemTypeName, opt => opt.MapFrom(src => src.ItemType.Name))
                .ForMember(dest => dest.SizeName, opt => opt.MapFrom(src => src.Size.Value));
            CreateMap<CreateItemDTO, Item>();
            CreateMap<UpdateItemDTO, Item>();

            CreateMap<Inventory, InventoryDTO>()
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item.Name))
                .ForMember(dest => dest.SizeName, opt => opt.MapFrom(src => src.Item.Size.Value));

            CreateMap<ItemTypeEmployeeType, ItemTypeEmployeeTypeDTO>()
                .ForMember(dest => dest.ItemTypeName, opt => opt.MapFrom(src => src.ItemType.Name))
                .ForMember(dest => dest.EmployeeTypeName, opt => opt.MapFrom(src => src.EmployeeType.Type));
            CreateMap<CreateItemTypeEmployeeTypeDTO, ItemTypeEmployeeType>();

            CreateMap<WarehouseMovement, WarehouseMovementDTO>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => 
                    src.Employee != null ? src.Employee.Name : null));

            CreateMap<CreateWarehouseMovementDTO, WarehouseMovement>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<WarehouseMovementDetail, WarehouseMovementDetailDTO>()
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item.Name))
                .ForMember(dest => dest.SizeName, opt => opt.MapFrom(src => src.Item.Size.Value));

            CreateMap<CreateWarehouseMovementDetailDTO, WarehouseMovementDetail>();
        }
    }
}
