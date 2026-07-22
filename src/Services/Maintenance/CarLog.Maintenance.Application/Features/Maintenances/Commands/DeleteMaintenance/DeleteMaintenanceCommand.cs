using MediatR;

namespace CarLog.Maintenance.Application.Features.Maintenances.Commands.DeleteMaintenance;

public sealed record DeleteMaintenanceCommand(Guid Id) : IRequest;
