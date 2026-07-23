using FluentValidation;

namespace CarLog.Reminder.Application.Features.Reminders.Commands.MarkReminderCompleted;

public class MarkReminderCompletedCommandValidator : AbstractValidator<MarkReminderCompletedCommand>
{
    public MarkReminderCompletedCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    }
}