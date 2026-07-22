using CarLog.Vehicle.Domain.Enums;
using CarLog.Vehicle.Domain.Exceptions;
using CarLog.Vehicle.Domain.ValueObjects;

namespace CarLog.Vehicle.Domain.Entities;

public sealed class VehicleEntity
{
    public Guid Id { get; private set; }
    public string Make { get; private set; } = null!;
    public string Model { get; private set; } = null!;
    public Year Year { get; private set; } = null!;
    public LicensePlate LicensePlate { get; private set; } = null!;
    public string? Vin { get; private set; }
    public VehicleType Type { get; private set; }
    public FuelType FuelType { get; private set; }
    public int EngineDisplacement { get; private set; }
    public int HorsePower { get; private set; }
    public OwnerType OwnerType { get; private set; }
    public Guid? OwnerId { get; private set; }
    public int CurrentMileage { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime UpdatedAtUtc { get; private set; }

    private VehicleEntity() { }

    private VehicleEntity(string make, string model, Year year, LicensePlate licensePlate, string? vin, VehicleType type, FuelType fuelType, int engineDisplacement, int horsePower, OwnerType ownerType, Guid? ownerId)
    {
        Id = Guid.NewGuid();
        Make = make;
        Model = model;
        Year = year;
        LicensePlate = licensePlate;
        Vin = vin;
        Type = type;
        FuelType = fuelType;
        EngineDisplacement = engineDisplacement;
        HorsePower = horsePower;
        OwnerType = ownerType;
        OwnerId = ownerId;
        CurrentMileage = 0;
        CreatedAtUtc = DateTime.UtcNow;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public static VehicleEntity Create(string make, string model, int year, string licensePlate, string countryCode, string? vin, VehicleType type, FuelType fuelType, int engineDisplacement, int horsePower, OwnerType ownerType, Guid? ownerId)
    {
        if (engineDisplacement <= 0) throw new DomainException("Engine displacement must be positive");

        if (horsePower <= 0) throw new DomainException("Horse power must be positive");

        return new VehicleEntity(make.Trim(), model.Trim(), Year.Create(year), LicensePlate.Create(licensePlate.Trim(), countryCode), vin?.Trim(), type, fuelType, engineDisplacement, horsePower, ownerType, ownerId);
    }

    public void UpdateMileage(int newMileage)
    {
        if (newMileage < CurrentMileage) throw new DomainException("Mileage cannot decrease");

        CurrentMileage = newMileage;

        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void UpdateDetails(string? make = null, string? model = null, int? year = null, VehicleType? type = null, FuelType? fuelType = null, int? engineDisplacement = null, int? horsePower = null)
    {
        if (make != null)
        {
            if (string.IsNullOrWhiteSpace(make)) throw new DomainException("Make cannot be empty");
            
            Make = make.Trim();
        }

        if (model != null)
        {
            if (string.IsNullOrWhiteSpace(model)) throw new DomainException("Model cannot be empty");
            
            Model = model.Trim();
        }

        if (year.HasValue) Year = Year.Create(year.Value);

        if (type.HasValue) Type = type.Value;
        
        if (fuelType.HasValue) FuelType = fuelType.Value;

        if (engineDisplacement.HasValue)
        {
            if (engineDisplacement <= 0) throw new DomainException("Engine displacement must be positive");

            EngineDisplacement = engineDisplacement.Value;
        }

        if (horsePower.HasValue)
        {
            if (horsePower <= 0) throw new DomainException("Horse power must be positive");

            HorsePower = horsePower.Value;
        }

        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void ChangeOwner(OwnerType newOwnerType, Guid? newOwnerId)
    {
        if (!newOwnerId.HasValue) throw new DomainException("OwnerId cannot be null when changing owner");

        OwnerType = newOwnerType;

        OwnerId = newOwnerId;
        
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void TransferToCompany(Guid companyId)
    {
        OwnerType = OwnerType.Corporate;

        OwnerId = companyId;
        
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void TransferToUser(Guid userId)
    {
        OwnerType = OwnerType.Personal;
        
        OwnerId = userId;
        
        UpdatedAtUtc = DateTime.UtcNow;
    }
}