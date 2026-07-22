using AutoMapper;
using CarLog.Maintenance.Domain.Entities;
using CarLog.Maintenance.Application.DTOs;

namespace CarLog.Maintenance.Application.Mappings;

public class MaintenanceMappingProfile : Profile
{
    public MaintenanceMappingProfile() 
    {
        CreateMap<MaintenanceEntity, MaintenanceDto>()
        .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
        .ForMember(dest => dest.CostAmount, opt => opt.MapFrom(src => src.Cost.Amount))
        .ForMember(dest => dest.CostCurrency, opt => opt.MapFrom(src => src.Cost.Currency));
    }
}
