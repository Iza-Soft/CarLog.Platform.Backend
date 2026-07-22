using CarLog.Maintenance.Application.DTOs;
using CarLog.Maintenance.Domain.Enums;
using MediatR;

namespace CarLog.Maintenance.Application.Features.Maintenances.Commands.CreateMaintenancе;

public sealed record CreateMaintenanceCommand(
    Guid VehicleId,
    MaintenanceType Type,
    string Description,
    DateOnly ServiceDate,
    int MileageAtService,
    decimal CostAmount,
    string CostCurrency) : IRequest<MaintenanceDto>;
