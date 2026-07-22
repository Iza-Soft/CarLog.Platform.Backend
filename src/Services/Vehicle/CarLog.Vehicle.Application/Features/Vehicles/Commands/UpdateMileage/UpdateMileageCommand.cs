using MediatR;
using CarLog.Vehicle.Application.DTOs;

namespace CarLog.Vehicle.Application.Features.Vehicles.Commands.UpdateMileage;

public sealed record UpdateMileageCommand(Guid Id, int Mileage) : IRequest<VehicleDto>;