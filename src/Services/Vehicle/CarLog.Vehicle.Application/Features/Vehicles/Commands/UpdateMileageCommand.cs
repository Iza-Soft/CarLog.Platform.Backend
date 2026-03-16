using AutoMapper;
using CarLog.Vehicle.Application.DTOs;
using CarLog.Vehicle.Application.Interfaces.Repositories;
using FluentValidation;
using MediatR;

namespace CarLog.Vehicle.Application.Features.Vehicles.Commands;

public class UpdateMileageCommand : IRequest<VehicleDto>
{
    public Guid Id { get; set; }

    public int Mileage { get; set; }
}

public class UpdateMileageCommandHandler : IRequestHandler<UpdateMileageCommand, VehicleDto> 
{
    private readonly IVehicleRepository _vehicleRepository;

    private readonly IMapper _mapper;

    public UpdateMileageCommandHandler(IVehicleRepository vehicleRepository, IMapper mapper)
    {
        _vehicleRepository = vehicleRepository;

        _mapper = mapper;
    }

    public async Task<VehicleDto> Handle(UpdateMileageCommand request, CancellationToken cancellationToken) 
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new KeyNotFoundException($"Vehicle with ID {request.Id} not found");

        vehicle.UpdateMileage(request.Mileage);
        
        await _vehicleRepository.UpdateAsync(vehicle, cancellationToken);

        return _mapper.Map<VehicleDto>(vehicle);
    }
}

public class UpdateMileageCommandValidator : AbstractValidator<UpdateMileageCommand> 
{
    public UpdateMileageCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Vehicle ID is required");

        RuleFor(v => v.Mileage)
            .GreaterThanOrEqualTo(0).WithMessage("Mileage must be greater than or equal to 0");
    }
}