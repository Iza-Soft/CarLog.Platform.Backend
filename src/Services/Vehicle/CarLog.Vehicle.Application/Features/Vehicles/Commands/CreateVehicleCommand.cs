using AutoMapper;
using CarLog.Vehicle.Application.DTOs;
using CarLog.Vehicle.Application.Interfaces.Repositories;
using CarLog.Vehicle.Domain.Entities;
using CarLog.Vehicle.Domain.Enums;
using FluentValidation;
using MediatR;

namespace CarLog.Vehicle.Application.Features.Vehicles.Commands;

public class CreateVehicleCommand : IRequest<VehicleDto>
{
    public string Make { get; set; } = null!;

    public string Model { get; set; } = null!;

    public int Year { get; set; }

    public string LicensePlate { get; set; } = null!;

    public string CountryCode { get; set; } = null!;

    public string? Vin { get; set; }

    public string Type { get; set; } = null!;

    public string FuelType { get; set; } = null!;

    public int EngineDisplacement { get; set; }

    public int HorsePower { get; set; }

    public string OwnerType { get; set; } = null!;

    public Guid? OwnerId { get; set; }
}

public class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, VehicleDto>
{
    private readonly IVehicleRepository _vehicleRepository;
    
    private readonly IMapper _mapper;

    public CreateVehicleCommandHandler(IVehicleRepository vehicleRepository, IMapper mapper) 
    {
        _vehicleRepository = vehicleRepository;
        _mapper = mapper;
    }

    public async Task<VehicleDto> Handle(CreateVehicleCommand request, CancellationToken cancellationToken) 
    {
        var exists = await _vehicleRepository.LicensePlateExistsAsync(request.LicensePlate, request.CountryCode, cancellationToken);

        if (exists) throw new InvalidOperationException("Vehicle with this license plate already exists");

        var vehicle = VehicleEntity.Create(
            request.Make,
            request.Model,
            request.Year,
            request.LicensePlate,
            request.CountryCode,
            request.Vin,
            Enum.Parse<VehicleType>(request.Type),
            Enum.Parse<FuelType>(request.FuelType),
            request.EngineDisplacement,
            request.HorsePower,
            Enum.Parse<OwnerType>(request.OwnerType),
            request.OwnerId);

        var result = await _vehicleRepository.AddAsync(vehicle, cancellationToken);

        return _mapper.Map<VehicleDto>(result);
    }
}

public class CreateVehicleCommandValidator : AbstractValidator<CreateVehicleCommand> 
{
    public CreateVehicleCommandValidator()
    {
        RuleFor(v => v.Make)
            .NotEmpty().WithMessage("Make is required")
            .MaximumLength(50);

        RuleFor(v => v.Model)
            .NotEmpty().WithMessage("Model is required")
            .MaximumLength(50);

        RuleFor(v => v.Year)
            .InclusiveBetween(1900, DateTime.UtcNow.Year + 1)
            .WithMessage($"Year must be between 1900 and {DateTime.UtcNow.Year + 1}");

        RuleFor(v => v.LicensePlate)
            .NotEmpty().WithMessage("License plate is required");

        RuleFor(v => v.CountryCode)
            .NotEmpty().WithMessage("Country code is required")
            .Length(2).WithMessage("Country code must be 2 characters");

        RuleFor(v => v.Type)
            .NotEmpty().WithMessage("Vehicle type is required")
            .Must(type => Enum.TryParse<Domain.Enums.VehicleType>(type, out _))
            .WithMessage("Invalid vehicle type");

        RuleFor(v => v.FuelType)
            .NotEmpty().WithMessage("Fuel type is required")
            .Must(type => Enum.TryParse<Domain.Enums.FuelType>(type, out _))
            .WithMessage("Invalid fuel type");

        RuleFor(v => v.EngineDisplacement)
            .GreaterThan(0).WithMessage("Engine displacement must be greater than 0");

        RuleFor(v => v.HorsePower)
            .GreaterThan(0).WithMessage("Horse power must be greater than 0");

        RuleFor(v => v.OwnerType)
            .NotEmpty().WithMessage("Owner type is required")
            .Must(type => Enum.TryParse<Domain.Enums.OwnerType>(type, out _))
            .WithMessage("Invalid owner type");

        When(v => v.OwnerType == Domain.Enums.OwnerType.Corporate.ToString(), () =>
        {
            RuleFor(v => v.OwnerId)
                .NotNull().WithMessage("OwnerId is required for corporate vehicles");
        });
    }
}