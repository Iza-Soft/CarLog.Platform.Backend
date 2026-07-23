using MediatR;

namespace CarLog.Reminder.Application.Features.Reminders.Commands.DeleteReminder;

public sealed record DeleteReminderCommand(Guid Id) : IRequest;