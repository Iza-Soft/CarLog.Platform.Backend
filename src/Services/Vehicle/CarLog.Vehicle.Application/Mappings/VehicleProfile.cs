using AutoMapper;
using CarLog.Vehicle.Application.DTOs;
using CarLog.Vehicle.Domain.Entities;

namespace CarLog.Vehicle.Application.Mappings;

public class VehicleProfile : Profile
{
    public VehicleProfile () 
    {
        CreateMap<VehicleEntity, VehicleDto>()
            .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year.Value))
            .ForMember(dest => dest.LicensePlate, opt => opt.MapFrom(src => src.LicensePlate.PlateNumber))
            .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.LicensePlate.CountryCode))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ForMember(dest => dest.FuelType, opt => opt.MapFrom(src => src.FuelType.ToString()))
            .ForMember(dest => dest.OwnerType, opt => opt.MapFrom(src => src.OwnerType.ToString()));

        CreateMap<CreateVehicleDto, VehicleEntity>()
            .ConstructUsing(dto => VehicleEntity.Create(
                dto.Make,
                dto.Model,
                dto.Year,
                dto.LicensePlate,
                dto.CountryCode,
                dto.Vin,
                Enum.Parse<Domain.Enums.VehicleType>(dto.Type),
                Enum.Parse<Domain.Enums.FuelType>(dto.FuelType),
                dto.EngineDisplacement,
                dto.HorsePower,
                Enum.Parse<Domain.Enums.OwnerType>(dto.OwnerType),
                dto.OwnerId));
    }
}
