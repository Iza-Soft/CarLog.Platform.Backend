using AutoMapper;
using CarLog.Vehicle.Application.Common.Exceptions;
using CarLog.Vehicle.Application.Common.Interfaces;
using CarLog.Vehicle.Application.DTOs;
using CarLog.Vehicle.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CarLog.Vehicle.Application.Features.Vehicles.Commands.UpdateMileage;

public class UpdateMileageCommandHandler : IRequestHandler<UpdateMileageCommand, VehicleDto>
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateMileageCommandHandler> _logger;

    public UpdateMileageCommandHandler(IVehicleRepository vehicleRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateMileageCommandHandler> logger)
    {
        _vehicleRepository = vehicleRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<VehicleDto> Handle(UpdateMileageCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException(nameof(Domain.Entities.VehicleEntity), request.Id);

        vehicle.UpdateMileage(request.Mileage);

        await _vehicleRepository.UpdateAsync(vehicle, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Updated mileage for vehicle {VehicleId}: {PreviousMileage} -> {NewMileage}", vehicle.Id, vehicle.CurrentMileage, request.Mileage);

        return _mapper.Map<VehicleDto>(vehicle);
    }
}