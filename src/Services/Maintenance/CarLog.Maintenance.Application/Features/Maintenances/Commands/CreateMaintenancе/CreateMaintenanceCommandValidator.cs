using FluentValidation;

namespace CarLog.Maintenance.Application.Features.Maintenances.Commands.CreateMaintenancе;

public class CreateMaintenanceCommandValidator : AbstractValidator<CreateMaintenanceCommand>
{
    public CreateMaintenanceCommandValidator()
    {
        RuleFor(x => x.VehicleId).NotEmpty().WithMessage("VehicleId is required.");

        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is Required.").MaximumLength(500).WithMessage("The description cannot exceed 500 characters.");

        RuleFor(x => x.ServiceDate).NotEmpty().LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage("Service date cannot be in the future.");

        RuleFor(x => x.MileageAtService).GreaterThanOrEqualTo(0).WithMessage("Mileage cannot be negative.");

        RuleFor(x => x.CostAmount).GreaterThanOrEqualTo(0).WithMessage("The amount cannot be negative.");

        RuleFor(x => x.CostCurrency).NotEmpty().Length(3).WithMessage("Currency must be a 3-letter code (e.g. BGN, EUR).");
    }
}
