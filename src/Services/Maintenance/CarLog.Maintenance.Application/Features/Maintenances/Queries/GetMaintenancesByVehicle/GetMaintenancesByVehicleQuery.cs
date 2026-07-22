using MediatR;
using CarLog.Maintenance.Application.DTOs;

namespace CarLog.Maintenance.Application.Features.Maintenances.Queries.GetMaintenancesByVehicle;

public sealed record GetMaintenancesByVehicleQuery(Guid VehicleId) : IRequest<IReadOnlyList<MaintenanceDto>>;