namespace CarLog.Vehicle.Application.DTOs;

public record VehicleDto
{
    public Guid Id { get; init; }

    public string Make { get; init; } = null!;

    public string Model { get; init; } = null!;

    public int Year { get; init; }

    public string LicensePlate { get; init; } = null!;

    public string CountryCode { get; init; } = null!;

    public string? Vin { get; init; }

    public string Type { get; init; } = null!;

    public string FuelType { get; init; } = null!;

    public int EngineDisplacement { get; init; }

    public int HorsePower { get; init; }

    public string OwnerType { get; init; } = null!;

    public Guid? OwnerId { get; init; }

    public int CurrentMileage { get; init; }

    public DateTime CreatedAt { get; init; }
}
