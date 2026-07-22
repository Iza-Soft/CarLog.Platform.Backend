using CarLog.Maintenance.Application.DTOs;
using CarLog.Maintenance.Domain.Enums;
using MediatR;

namespace CarLog.Maintenance.Application.Features.Maintenances.Commands.UpdateMaintenance;

public sealed record UpdateMaintenanceCommand(
    Guid Id,
    MaintenanceType Type,
    string Description,
    DateOnly ServiceDate,
    int MileageAtService,
    decimal CostAmount,
    string CostCurrency) : IRequest<MaintenanceDto>;
