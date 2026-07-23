namespace CarLog.Reminder.Application.DTOs;

public sealed record ReminderDto(
    Guid Id,
    Guid VehicleId,
    string Type,
    string Description,
    DateOnly? DueDate,
    int? DueMileage,
    bool IsCompleted,
    bool IsOverdue,
    DateTime CreatedAtUtc);