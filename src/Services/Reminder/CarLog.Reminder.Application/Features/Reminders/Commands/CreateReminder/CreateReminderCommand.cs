using MediatR;
using CarLog.Reminder.Application.DTOs;
using CarLog.Reminder.Domain.Enums;

namespace CarLog.Reminder.Application.Features.Reminders.Commands.CreateReminder;

public sealed record CreateReminderCommand(
    Guid VehicleId,
    ReminderType Type,
    string Description,
    DateOnly? DueDate,
    int? DueMileage) : IRequest<ReminderDto>;