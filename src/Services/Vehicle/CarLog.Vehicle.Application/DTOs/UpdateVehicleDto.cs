namespace CarLog.Vehicle.Application.DTOs;

public record UpdateVehicleDto
{
    public string? Make { get; init; }

    public string? Model { get; init; }

    public int? Year { get; init; }

    public string? Type { get; init; }

    public string? FuelType { get; init; }

    public int? EngineDisplacement { get; init; }

    public int? HorsePower { get; init; }
}
