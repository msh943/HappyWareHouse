using AutoMapper;
using HappyWarehouse.Domain.Dto;
using HappyWarehouse.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            CreateMap<UpdateUserDto, User>();

            CreateMap<Warehouse, WarehouseDto>()
           .ForMember(d => d.CountryName, m => m.MapFrom(s => s.Country != null ? s.Country.Name : string.Empty))
           .ForMember(d => d.WarehouseItems,m => m.MapFrom(s => s.Items));

            CreateMap<WarehouseItem, WarehouseItemDto>();

            CreateMap<CreateWarehouseDto, Warehouse>();


            CreateMap<UpdateWarehouseDto, Warehouse>();


            CreateMap<CreateWarehouseItemDto, WarehouseItem>();


            CreateMap<UpdateWarehouseItemDto, WarehouseItem>();
        }
    }
}
