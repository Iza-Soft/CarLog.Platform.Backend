using FluentValidation;
using CarLog.Expense.Domain.Enums;

namespace CarLog.Expense.Application.Features.Expenses.Commands.CreateExpense;

public class CreateExpenseCommandValidator : AbstractValidator<CreateExpenseCommand>
{
    public CreateExpenseCommandValidator()
    {
        RuleFor(x => x.VehicleId).NotEmpty().WithMessage("VehicleId is required.");

        RuleFor(x => x.Type).NotEqual(ExpenseType.Unknown).WithMessage("Expense type must be specified.");

        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.").MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

        RuleFor(x => x.ExpenseDate).NotEmpty().LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage("Expense date cannot be in the future.");

        RuleFor(x => x.AmountValue).GreaterThanOrEqualTo(0).WithMessage("Amount cannot be negative.");

        RuleFor(x => x.AmountCurrency).NotEmpty().Length(3).WithMessage("Currency must be a 3-letter code (e.g. BGN, EUR).");
    }
}