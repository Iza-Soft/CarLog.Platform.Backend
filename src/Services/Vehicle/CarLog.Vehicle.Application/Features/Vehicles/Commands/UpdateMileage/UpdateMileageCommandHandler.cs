using AutoMapper;
using CarLog.Vehicle.Application.Common.Exceptions;
using CarLog.Vehicle.Application.DTOs;
using CarLog.Vehicle.Application.Interfaces;
using MediatR;

namespace CarLog.Vehicle.Application.Features.Vehicles.Commands.UpdateMileage;

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
        var vehicle = await _vehicleRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException(nameof(Domain.Entities.VehicleEntity), request.Id);

        vehicle.UpdateMileage(request.Mileage);

        await _vehicleRepository.UpdateAsync(vehicle, cancellationToken);

        return _mapper.Map<VehicleDto>(vehicle);
    }
}