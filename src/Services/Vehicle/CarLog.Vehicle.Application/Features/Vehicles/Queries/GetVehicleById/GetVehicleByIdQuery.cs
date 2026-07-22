using MediatR;
using CarLog.Vehicle.Application.DTOs;

namespace CarLog.Vehicle.Application.Features.Vehicles.Queries.GetVehicleById;

public sealed record GetVehicleByIdQuery(Guid Id) : IRequest<VehicleDto>;