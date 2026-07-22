using MediatR;
using CarLog.Vehicle.Application.DTOs;
using CarLog.Vehicle.Domain.Enums;

namespace CarLog.Vehicle.Application.Features.Vehicles.Commands.UpdateVehicle;

public sealed record UpdateVehicleCommand(
    Guid Id,
    string? Make,
    string? Model,
    int? Year,
    VehicleType? Type,
    FuelType? FuelType,
    int? EngineDisplacement,
    int? HorsePower) : IRequest<VehicleDto>;