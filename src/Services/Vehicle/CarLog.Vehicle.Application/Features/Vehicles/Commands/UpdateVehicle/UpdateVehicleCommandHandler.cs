using AutoMapper;
using CarLog.Vehicle.Application.Common.Exceptions;
using CarLog.Vehicle.Application.Common.Interfaces;
using CarLog.Vehicle.Application.DTOs;
using CarLog.Vehicle.Application.Interfaces;
using MediatR;

namespace CarLog.Vehicle.Application.Features.Vehicles.Commands.UpdateVehicle;

public class UpdateVehicleCommandHandler : IRequestHandler<UpdateVehicleCommand, VehicleDto>
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateVehicleCommandHandler(IVehicleRepository vehicleRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _vehicleRepository = vehicleRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<VehicleDto> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException(nameof(Domain.Entities.VehicleEntity), request.Id);

        vehicle.UpdateDetails(
            request.Make,
            request.Model,
            request.Year,
            request.Type,
            request.FuelType,
            request.EngineDisplacement,
            request.HorsePower);

        await _vehicleRepository.UpdateAsync(vehicle, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<VehicleDto>(vehicle);
    }
}