using FluentValidation;

namespace CarLog.Vehicle.Application.Features.Vehicles.Commands.UpdateVehicle;

public class UpdateVehicleCommandValidator : AbstractValidator<UpdateVehicleCommand>
{
    public UpdateVehicleCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithMessage("Vehicle ID is required");

        When(v => v.Year.HasValue, () =>
        {
            RuleFor(v => v.Year!.Value).InclusiveBetween(1900, DateTime.UtcNow.Year + 1).WithMessage($"Year must be between 1900 and {DateTime.UtcNow.Year + 1}");
        });
    }
}