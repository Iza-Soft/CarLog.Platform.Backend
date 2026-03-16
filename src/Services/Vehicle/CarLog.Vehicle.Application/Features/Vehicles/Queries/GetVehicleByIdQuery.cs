using AutoMapper;
using CarLog.Vehicle.Application.DTOs;
using CarLog.Vehicle.Application.Interfaces.Repositories;
using MediatR;

namespace CarLog.Vehicle.Application.Features.Vehicles.Queries;

public class GetVehicleByIdQuery : IRequest<VehicleDto>
{
    public Guid Id { get; set; }
}

public class GetVehicleByIdQueryHandler : IRequestHandler<GetVehicleByIdQuery, VehicleDto?> 
{
    private readonly IVehicleRepository _vehicleRepository;

    private readonly IMapper _mapper;

    public GetVehicleByIdQueryHandler(IVehicleRepository vehicleRepository, IMapper mapper)
    {
        _vehicleRepository = vehicleRepository;

        _mapper = mapper;
    }

    public async Task<VehicleDto?> Handle(GetVehicleByIdQuery request, CancellationToken cancellationToken) 
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(request.Id, cancellationToken);

        return vehicle == null ? null : _mapper.Map<VehicleDto>(vehicle);
    }
}