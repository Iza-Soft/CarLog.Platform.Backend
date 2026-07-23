using FluentValidation;
using CarLog.Reminder.Domain.Enums;

namespace CarLog.Reminder.Application.Features.Reminders.Commands.UpdateReminder;

public class UpdateReminderCommandValidator : AbstractValidator<UpdateReminderCommand>
{
    public UpdateReminderCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.Type).NotEqual(ReminderType.Unknown).WithMessage("Reminder type must be specified.");

        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.").MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

        RuleFor(x => x).Must(x => x.DueDate.HasValue || x.DueMileage.HasValue).WithMessage("At least one of DueDate or DueMileage must be specified.");

        When(x => x.DueMileage.HasValue, () =>
        {
            RuleFor(x => x.DueMileage!.Value).GreaterThanOrEqualTo(0).WithMessage("DueMileage cannot be negative.");
        });
    }
}