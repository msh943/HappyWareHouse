using AutoMapper;
using HappyWarehouse.Domain.Dto;
using HappyWarehouse.Domain.Entities;

namespace HappyWarehouse.Service.Mapping
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<User, UserDto>()
                .ForMember(d => d.RoleName, m => m.MapFrom(s => s.Role != null ? s.Role.Name : string.Empty))
                .ForMember(d => d.IsActive, m => m.MapFrom(s => s.IsActive));

            CreateMap<CreateUserDto, User>();

            CreateMap<UpdateUserDto, User>().ForMember(d => d.Id, opt => opt.Ignore());

            CreateMap<Warehouse, WarehouseDto>()
           .ForMember(d => d.CountryName, m => m.MapFrom(s => s.Country != null ? s.Country.Name : string.Empty))
           .ForMember(d => d.WarehouseItems, m => m.MapFrom(s => s.Items));

            CreateMap<WarehouseItem, WarehouseItemDto>();

            CreateMap<CreateWarehouseDto, Warehouse>();


            CreateMap<UpdateWarehouseDto, Warehouse>().ForMember(d => d.Id, opt => opt.Ignore());


            CreateMap<CreateWarehouseItemDto, WarehouseItem>();


            CreateMap<UpdateWarehouseItemDto, WarehouseItem>().ForMember(d => d.Id, opt => opt.Ignore());

        }
    }
}
