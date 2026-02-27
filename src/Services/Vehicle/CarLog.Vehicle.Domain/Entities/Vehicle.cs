using CarLog.Vehicle.Domain.Common;
using CarLog.Vehicle.Domain.Enums;
using CarLog.Vehicle.Domain.ValueObjects;

namespace CarLog.Vehicle.Domain.Entities;

public class Vehicle : BaseEntity
{
    public string Make { get; private set; } = null!;
    public string Model { get; private set; } = null!;
    public Year Year { get; private set; } = null!;
    public LicensePlate LicensePlate { get; private set; } = null!;
    public string? Vin { get; private set; }
    public VehicleType Type { get; private set; }
    public FuelType FuelType { get; private set; }
    public int EngineDisplacement { get; private set; } // in cc
    public int HorsePower { get; private set; }
    public OwnerType OwnerType { get; private set; }
    public Guid? OwnerId { get; private set; } // B2C: UserId, B2B: CompanyId
    public int CurrentMileage { get; private set; }

    private Vehicle() { } // For EF Core

    public static Vehicle Create(
        string make,
        string model,
        int year,
        string licensePlate,
        string countryCode,
        string? vin,
        VehicleType type,
        FuelType fuelType,
        int engineDisplacement,
        int horsePower,
        OwnerType ownerType,
        Guid? ownerId)
    {
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            Make = make,
            Model = model,
            Year = Year.Create(year),
            LicensePlate = LicensePlate.Create(licensePlate, countryCode),
            Vin = vin,
            Type = type,
            FuelType = fuelType,
            EngineDisplacement = engineDisplacement,
            HorsePower = horsePower,
            OwnerType = ownerType,
            OwnerId = ownerId,
            CurrentMileage = 0,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        return vehicle;
    }

    public void UpdateMileage(int newMileage)
    {
        if (newMileage < CurrentMileage)
            throw new InvalidOperationException("Mileage cannot decrease");

        CurrentMileage = newMileage;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(
        string? make = null,
        string? model = null,
        int? year = null,
        VehicleType? type = null,
        FuelType? fuelType = null,
        int? engineDisplacement = null,
        int? horsePower = null)
    {
        if (make != null) Make = make;
        if (model != null) Model = model;
        if (year.HasValue) Year = Year.Create(year.Value);
        if (type.HasValue) Type = type.Value;
        if (fuelType.HasValue) FuelType = fuelType.Value;
        if (engineDisplacement.HasValue) EngineDisplacement = engineDisplacement.Value;
        if (horsePower.HasValue) HorsePower = horsePower.Value;

        UpdatedAt = DateTime.UtcNow;
    }

    public void Delete()
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }
}