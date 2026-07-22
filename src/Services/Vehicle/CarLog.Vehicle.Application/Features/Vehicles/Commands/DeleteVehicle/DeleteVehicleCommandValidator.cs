using FluentValidation;

namespace CarLog.Vehicle.Application.Features.Vehicles.Commands.DeleteVehicle;

public class DeleteVehicleCommandValidator : AbstractValidator<DeleteVehicleCommand>
{
    public DeleteVehicleCommandValidator()
    {
        RuleFor(v => v.Id).NotEmpty().WithMessage("Vehicle ID is required");
    }
}