using CarLog.Maintenance.Application.DTOs;
using MediatR;

namespace CarLog.Maintenance.Application.Features.Maintenances.Queries.GetMaintenanceById;

public sealed record GetMaintenanceByIdQuery(Guid Id) : IRequest<MaintenanceDto>;