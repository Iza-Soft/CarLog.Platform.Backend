using MediatR;
using CarLog.Reminder.Application.DTOs;

namespace CarLog.Reminder.Application.Features.Reminders.Queries.GetReminderById;

public sealed record GetReminderByIdQuery(Guid Id) : IRequest<ReminderDto>;