using FluentValidation;

namespace CarLog.Maintenance.Application.Features.Maintenances.Commands.UpdateMaintenance;

public class UpdateMaintenanceCommandValidator : AbstractValidator<UpdateMaintenanceCommand>
{
    public UpdateMaintenanceCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.").MaximumLength(500).WithMessage("The description cannot exceed 500 characters.");

        RuleFor(x => x.ServiceDate).NotEmpty().LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage("Service date cannot be in the future.");

        RuleFor(x => x.MileageAtService).GreaterThanOrEqualTo(0).WithMessage("Mileage cannot be negative.");

        RuleFor(x => x.CostAmount).GreaterThanOrEqualTo(0).WithMessage("Amount cannot be negative.");

        RuleFor(x => x.CostCurrency).NotEmpty().Length(3).WithMessage("Currency must be a 3-letter code (e.g. BGN, EUR).");
    }
}
