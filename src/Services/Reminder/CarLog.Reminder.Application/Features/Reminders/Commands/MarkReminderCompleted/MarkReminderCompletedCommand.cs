using MediatR;
using CarLog.Reminder.Application.DTOs;

namespace CarLog.Reminder.Application.Features.Reminders.Commands.MarkReminderCompleted;

public sealed record MarkReminderCompletedCommand(Guid Id) : IRequest<ReminderDto>;