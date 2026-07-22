using FluentValidation;
using CarLog.Vehicle.Domain.Enums;

namespace CarLog.Vehicle.Application.Features.Vehicles.Commands.CreateVehicle;

public class CreateVehicleCommandValidator : AbstractValidator<CreateVehicleCommand>
{
    public CreateVehicleCommandValidator()
    {
        RuleFor(v => v.Make).NotEmpty().WithMessage("Make is required").MaximumLength(50);

        RuleFor(v => v.Model).NotEmpty().WithMessage("Model is required").MaximumLength(50);

        RuleFor(v => v.Year).InclusiveBetween(1900, DateTime.UtcNow.Year + 1).WithMessage($"Year must be between 1900 and {DateTime.UtcNow.Year + 1}");

        RuleFor(v => v.LicensePlate).NotEmpty().WithMessage("License plate is required");

        RuleFor(v => v.CountryCode).NotEmpty().WithMessage("Country code is required").Length(2).WithMessage("Country code must be 2 characters");

        RuleFor(v => v.EngineDisplacement).GreaterThan(0).WithMessage("Engine displacement must be greater than 0");

        RuleFor(v => v.HorsePower).GreaterThan(0).WithMessage("Horse power must be greater than 0");

        When(v => v.OwnerType == OwnerType.Corporate, () =>
        {
            RuleFor(v => v.OwnerId).NotNull().WithMessage("OwnerId is required for corporate vehicles");
        });
    }
}