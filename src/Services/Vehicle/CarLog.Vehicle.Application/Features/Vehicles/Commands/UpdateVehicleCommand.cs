using AutoMapper;
using CarLog.Vehicle.Application.DTOs;
using CarLog.Vehicle.Application.Interfaces.Repositories;
using CarLog.Vehicle.Domain.Enums;
using FluentValidation;
using MediatR;

namespace CarLog.Vehicle.Application.Features.Vehicles.Commands;

public class UpdateVehicleCommand : IRequest<VehicleDto>
{
    public Guid Id { get; set; }

    public string? Make { get; set; }

    public string? Model { get; set; }

    public int? Year { get; set; }

    public string? Type { get; set; }

    public string? FuelType { get; set; }

    public int? EngineDisplacement { get; set; }

    public int? HorsePower { get; set; }
}

public class UpdateVehicleCommandHandler : IRequestHandler<UpdateVehicleCommand, VehicleDto> 
{
    private readonly IVehicleRepository _vehicleRepository;

    private readonly IMapper _mapper;

    public UpdateVehicleCommandHandler(IVehicleRepository vehicleRepository, IMapper mapper) 
    {
        _vehicleRepository = vehicleRepository;

        _mapper = mapper;
    }

    public async Task<VehicleDto> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken) 
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new Exception($"Vehicle with ID {request.Id} not found");

        vehicle.UpdateDetails(
            request.Make,
            request.Model,
            request.Year,
            request.Type != null ? Enum.Parse<VehicleType>(request.Type) : null,
            request.FuelType != null ? Enum.Parse<FuelType>(request.FuelType) : null,
            request.EngineDisplacement,
            request.HorsePower);

        await _vehicleRepository.UpdateAsync(vehicle, cancellationToken);

        return _mapper.Map<VehicleDto>(vehicle);
    }
}

public class UpdateVehicleCommandValidator : AbstractValidator<UpdateVehicleCommand> 
{
    public UpdateVehicleCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Vehicle ID is required");

        When(v => v.Year.HasValue, () =>
        {
            RuleFor(v => v.Year!.Value)
                .InclusiveBetween(1900, DateTime.UtcNow.Year + 1)
                .WithMessage($"Year must be between 1900 and {DateTime.UtcNow.Year + 1}");
        });
    }
}