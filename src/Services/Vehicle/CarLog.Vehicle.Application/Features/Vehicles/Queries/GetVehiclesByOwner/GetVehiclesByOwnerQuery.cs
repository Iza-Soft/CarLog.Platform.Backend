using MediatR;
using CarLog.Vehicle.Application.DTOs;
using CarLog.Vehicle.Domain.Enums;

namespace CarLog.Vehicle.Application.Features.Vehicles.Queries.GetVehiclesByOwner;

public sealed record GetVehiclesByOwnerQuery(Guid OwnerId, OwnerType OwnerType) : IRequest<IReadOnlyList<VehicleDto>>;