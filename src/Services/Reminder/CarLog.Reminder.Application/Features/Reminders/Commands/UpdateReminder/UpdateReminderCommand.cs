using MediatR;
using CarLog.Reminder.Application.DTOs;
using CarLog.Reminder.Domain.Enums;

namespace CarLog.Reminder.Application.Features.Reminders.Commands.UpdateReminder;

public sealed record UpdateReminderCommand(
    Guid Id,
    ReminderType Type,
    string Description,
    DateOnly? DueDate,
    int? DueMileage) : IRequest<ReminderDto>;