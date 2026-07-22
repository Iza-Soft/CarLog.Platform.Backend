using CarLog.Vehicle.Application.Common.Exceptions;
using CarLog.Vehicle.Application.Interfaces;
using MediatR;

namespace CarLog.Vehicle.Application.Features.Vehicles.Commands.DeleteVehicle;

public class DeleteVehicleCommandHandler : IRequestHandler<DeleteVehicleCommand>
{
    private readonly IVehicleRepository _vehicleRepository;

    public DeleteVehicleCommandHandler(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException(nameof(Domain.Entities.VehicleEntity), request.Id);

        await _vehicleRepository.DeleteAsync(vehicle, cancellationToken);
    }
}