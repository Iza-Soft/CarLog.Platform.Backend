using CarLog.Vehicle.Application.Interfaces.Repositories;
using FluentValidation;
using MediatR;

namespace CarLog.Vehicle.Application.Features.Vehicles.Commands;

public class DeleteVehicleCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}

public class DeleteVehicleCommandHandler : IRequestHandler<DeleteVehicleCommand, Unit> 
{
    private readonly IVehicleRepository _vehicleRepository;

    public DeleteVehicleCommandHandler(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<Unit> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken) 
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new KeyNotFoundException($"Vehicle with ID {request.Id} not found");

        vehicle.Delete();

        await _vehicleRepository.UpdateAsync(vehicle, cancellationToken);

        return Unit.Value;
    }
}

public class DeleteVehicleCommandValidator : AbstractValidator<DeleteVehicleCommand> 
{
    public DeleteVehicleCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Vehicle ID is required");
    }
}
