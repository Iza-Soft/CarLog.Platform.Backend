using FluentValidation;

namespace CarLog.Reminder.Application.Features.Reminders.Commands.DeleteReminder;

public class DeleteReminderCommandValidator : AbstractValidator<DeleteReminderCommand>
{
    public DeleteReminderCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    }
}