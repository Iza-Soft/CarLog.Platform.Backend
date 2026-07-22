using MediatR;
using CarLog.Vehicle.Application.DTOs;
using CarLog.Vehicle.Domain.Enums;

namespace CarLog.Vehicle.Application.Features.Vehicles.Commands.CreateVehicle;

public sealed record CreateVehicleCommand(
    string Make,
    string Model,
    int Year,
    string LicensePlate,
    string CountryCode,
    string? Vin,
    VehicleType Type,
    FuelType FuelType,
    int EngineDisplacement,
    int HorsePower,
    OwnerType OwnerType,
    Guid? OwnerId) : IRequest<VehicleDto>;