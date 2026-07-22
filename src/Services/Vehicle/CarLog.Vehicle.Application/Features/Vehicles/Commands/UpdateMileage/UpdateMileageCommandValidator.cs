using FluentValidation;

namespace CarLog.Vehicle.Application.Features.Vehicles.Commands.UpdateMileage;

public class UpdateMileageCommandValidator : AbstractValidator<UpdateMileageCommand>
{
    public UpdateMileageCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithMessage("Vehicle ID is required");

        RuleFor(v => v.Mileage).GreaterThanOrEqualTo(0).WithMessage("Mileage must be greater than or equal to 0");
    }
}