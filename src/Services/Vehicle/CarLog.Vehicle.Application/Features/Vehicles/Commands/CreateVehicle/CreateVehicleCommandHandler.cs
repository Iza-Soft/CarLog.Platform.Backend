using AutoMapper;
using MediatR;
using CarLog.Vehicle.Application.Common.Exceptions;
using CarLog.Vehicle.Application.DTOs;
using CarLog.Vehicle.Application.Interfaces;

namespace CarLog.Vehicle.Application.Features.Vehicles.Commands.CreateVehicle;

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

        return _mapper.Map<VehicleDto>(result);
    }
}