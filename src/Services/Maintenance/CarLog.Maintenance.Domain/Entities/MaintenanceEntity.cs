using CarLog.Maintenance.Domain.Enums;
using CarLog.Maintenance.Domain.Exceptions;
using CarLog.Maintenance.Domain.ValueObjects;

namespace CarLog.Maintenance.Domain.Entities;

public class MaintenanceEntity
{
    public Guid Id { get; private set; }
    
    public Guid VehicleId { get; private set; }
    
    public MaintenanceType Type { get; private set; }
    
    public string Description { get; private set; } = null!;
    
    public DateOnly ServiceDate { get; private set; }
    
    public int MileageAtService { get; private set; }
    
    public Money Cost { get; private set; } = null!;
    
    public DateTime CreatedAtUtc { get; private set; }

    // EF Core reqired parameterless constructor — private, because we don't want to use directly from code
    private MaintenanceEntity() { }

    public static MaintenanceEntity Create(Guid vehicleId, MaintenanceType type, string description, DateOnly serviceDate, int mileageAtService, Money cost) 
    {
        if (vehicleId == Guid.Empty) throw new DomainException("VehicleId is required.");

        if (string.IsNullOrWhiteSpace(description)) throw new DomainException("Description can not be empty.");

        if (serviceDate > DateOnly.FromDateTime(DateTime.UtcNow)) throw new DomainException("Service date cannot be in the future.");

        if (mileageAtService < 0) throw new DomainException("Mileage cannot be negative.");

        return new MaintenanceEntity
        {
            Id = Guid.NewGuid(),
            VehicleId = vehicleId,
            Type = type,
            Description = description.Trim(),
            ServiceDate = serviceDate,
            MileageAtService = mileageAtService,
            Cost = cost,
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    public void UpdateCost(Money newCost)
    {
        Cost = newCost;
    }

    public void UpdateDescription(string newDescription)
    {
        if (string.IsNullOrWhiteSpace(newDescription)) throw new DomainException("Description can not be empty.");

        Description = newDescription.Trim();
    }

    public void Update(MaintenanceType type, string description, DateOnly serviceDate, int mileageAtService, Money cost)
    {
        if (string.IsNullOrWhiteSpace(description)) throw new DomainException("Description can not be empty.");

        if (serviceDate > DateOnly.FromDateTime(DateTime.UtcNow)) throw new DomainException("Service date cannot be in the future.");

        if (mileageAtService < 0) throw new DomainException("Mileage cannot be negative.");

        Type = type;

        Description = description.Trim();
        
        ServiceDate = serviceDate;
        
        MileageAtService = mileageAtService;
        
        Cost = cost;
    }
}
