using MediatR;
using CarLog.Reminder.Application.DTOs;

namespace CarLog.Reminder.Application.Features.Reminders.Queries.GetRemindersByVehicle;

public sealed record GetRemindersByVehicleQuery(Guid VehicleId) : IRequest<IReadOnlyList<ReminderDto>>;