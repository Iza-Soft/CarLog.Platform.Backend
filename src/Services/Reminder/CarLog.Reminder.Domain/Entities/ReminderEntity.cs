using CarLog.Reminder.Domain.Enums;
using CarLog.Reminder.Domain.Exceptions;

namespace CarLog.Reminder.Domain.Entities;

public sealed class ReminderEntity
{
    public Guid Id { get; private set; }

    public Guid VehicleId { get; private set; }

    public ReminderType Type { get; private set; }

    public string Description { get; private set; } = string.Empty;

    public DateOnly? DueDate { get; private set; }

    public int? DueMileage { get; private set; }

    public bool IsCompleted { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    private ReminderEntity() { }

    public static ReminderEntity Create(Guid vehicleId, ReminderType type, string description, DateOnly? dueDate, int? dueMileage)
    {
        if (vehicleId == Guid.Empty) throw new DomainException("VehicleId is required.");

        if (string.IsNullOrWhiteSpace(description)) throw new DomainException("Description cannot be empty.");

        if (!dueDate.HasValue && !dueMileage.HasValue) throw new DomainException("At least one of DueDate or DueMileage must be specified.");

        if (dueMileage.HasValue && dueMileage.Value < 0) throw new DomainException("DueMileage cannot be negative.");

        return new ReminderEntity
        {
            Id = Guid.NewGuid(),
            VehicleId = vehicleId,
            Type = type,
            Description = description.Trim(),
            DueDate = dueDate,
            DueMileage = dueMileage,
            IsCompleted = false,
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    public void Update(ReminderType type, string description, DateOnly? dueDate, int? dueMileage)
    {
        if (string.IsNullOrWhiteSpace(description)) throw new DomainException("Description cannot be empty.");

        if (!dueDate.HasValue && !dueMileage.HasValue) throw new DomainException("At least one of DueDate or DueMileage must be specified.");

        if (dueMileage.HasValue && dueMileage.Value < 0) throw new DomainException("DueMileage cannot be negative.");

        Type = type;
        Description = description.Trim();
        DueDate = dueDate;
        DueMileage = dueMileage;
    }

    public void MarkCompleted()
    {
        if (IsCompleted) throw new DomainException("Reminder is already marked as completed.");

        IsCompleted = true;
    }

    public bool IsOverdue(DateOnly currentDate, int? currentMileage)
    {
        if (IsCompleted) return false;

        if (DueDate.HasValue && currentDate > DueDate.Value) return true;

        if (DueMileage.HasValue && currentMileage.HasValue && currentMileage.Value >= DueMileage.Value) return true;

        return false;
    }
}