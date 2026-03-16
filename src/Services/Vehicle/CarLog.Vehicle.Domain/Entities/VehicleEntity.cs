using CarLog.Vehicle.Domain.Common;
using CarLog.Vehicle.Domain.Enums;
using CarLog.Vehicle.Domain.ValueObjects;

namespace CarLog.Vehicle.Domain.Entities;

public sealed class VehicleEntity : BaseEntity
{
    public string Make { get; private set; } = null!;

    public string Model { get; private set; } = null!;

    public Year Year { get; private set; } = null!;

    public LicensePlate LicensePlate { get; private set; } = null!;

    public string? Vin { get; private set; }

    public VehicleType Type { get; private set; } // this is our "discriminator"

    public FuelType FuelType { get; private set; }

    public int EngineDisplacement { get; private set; }

    public int HorsePower { get; private set; }

    public OwnerType OwnerType { get; private set; }

    public Guid? OwnerId { get; private set; } // B2C: UserId, B2B: CompanyId

    public int CurrentMileage { get; private set; }

    #region Private constructor за EF Core (only for db creation)
    private VehicleEntity() { }
    #endregion

    #region Private business constructor
    private VehicleEntity(
        string make,
        string model,
        Year year,
        LicensePlate licensePlate,
        string? vin,
        VehicleType type,
        FuelType fuelType,
        int engineDisplacement,
        int horsePower,
        OwnerType ownerType,
        Guid? ownerId)
    {
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
    }
    #endregion

    #region public factory method - the only way to create a new Vehicle
    public static VehicleEntity Create(
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
        #region Validation
        if (engineDisplacement <= 0)
            throw new ArgumentException("Engine displacement must be positive", nameof(engineDisplacement));

        if (horsePower <= 0)
            throw new ArgumentException("Horse power must be positive", nameof(horsePower));
        #endregion

        return new VehicleEntity(
            make.Trim(),
            model.Trim(),
            Year.Create(year),
            LicensePlate.Create(licensePlate.Trim(), countryCode),
            vin?.Trim(),
            type,
            fuelType,
            engineDisplacement,
            horsePower,
            ownerType,
            ownerId);
    }
    #endregion

    #region Business methods
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
        if (make != null)
        {
            if (string.IsNullOrWhiteSpace(make))
                throw new ArgumentException("Make cannot be empty", nameof(make));
            Make = make.Trim();
        }

        if (model != null)
        {
            if (string.IsNullOrWhiteSpace(model))
                throw new ArgumentException("Model cannot be empty", nameof(model));
            Model = model.Trim();
        }

        if (year.HasValue) Year = Year.Create(year.Value);
        if (type.HasValue) Type = type.Value;
        if (fuelType.HasValue) FuelType = fuelType.Value;

        if (engineDisplacement.HasValue)
        {
            if (engineDisplacement <= 0)
                throw new ArgumentException("Engine displacement must be positive", nameof(engineDisplacement));
            EngineDisplacement = engineDisplacement.Value;
        }

        if (horsePower.HasValue)
        {
            if (horsePower <= 0)
                throw new ArgumentException("Horse power must be positive", nameof(horsePower));
            HorsePower = horsePower.Value;
        }

        UpdatedAt = DateTime.UtcNow;
    }

    public void Delete()
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeOwner(OwnerType newOwnerType, Guid? newOwnerId)
    {
        if (!newOwnerId.HasValue)
            throw new ArgumentException("OwnerId cannot be null when changing owner");

        OwnerType = newOwnerType;
        OwnerId = newOwnerId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void TransferToCompany(Guid companyId)
    {
        OwnerType = OwnerType.Corporate;
        OwnerId = companyId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void TransferToUser(Guid userId)
    {
        OwnerType = OwnerType.Personal;
        OwnerId = userId;
        UpdatedAt = DateTime.UtcNow;
    }
    #endregion
}