namespace CarLog.Maintenance.Application.DTOs;

public sealed record MaintenanceDto(
    Guid Id,
    Guid VehicleId,
    string Type,
    string Description,
    DateOnly ServiceDate,
    int MileageAtService,
    decimal CostAmount,
    string CostCurrency,
    DateTime CreatedAtUtc);