using AutoMapper;
using CarLog.Vehicle.Application.Common.Exceptions;
using CarLog.Vehicle.Application.Common.Interfaces;
using CarLog.Vehicle.Application.DTOs;
using CarLog.Vehicle.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CarLog.Vehicle.Application.Features.Vehicles.Commands.CreateVehicle;

public class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, VehicleDto>
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateVehicleCommandHandler> _logger;

    public CreateVehicleCommandHandler(IVehicleRepository vehicleRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateVehicleCommandHandler> logger)
    {
        _vehicleRepository = vehicleRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<VehicleDto> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        var exists = await _vehicleRepository.LicensePlateExistsAsync(request.LicensePlate, request.CountryCode, cancellationToken);

        if (exists) throw new ConflictException("Vehicle with this license plate already exists");

        var vehicle = Domain.Entities.VehicleEntity.Create(
            request.Make,
            request.Model,
            request.Year,
            request.LicensePlate,
            request.CountryCode,
            request.Vin,
            request.Type,
            request.FuelType,
            request.EngineDisplacement,
            request.HorsePower,
            request.OwnerType,
            request.OwnerId);

        var result = await _vehicleRepository.AddAsync(vehicle, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created vehicle {VehicleId}, {Make} {Model} ({Year}), plate {LicensePlate} ({CountryCode}), owner {OwnerId} ({OwnerType})", result.Id, request.Make, request.Model, request.Year, request.LicensePlate, request.CountryCode, request.OwnerId, request.OwnerType);

        return _mapper.Map<VehicleDto>(result);
    }
}