using AutoMapper;
using CarLog.Maintenance.Application.Common.Interfaces;
using CarLog.Maintenance.Application.DTOs;
using MediatR;

namespace CarLog.Maintenance.Application.Features.Maintenances.Queries.GetMaintenancesByVehicle;

public class GetMaintenancesByVehicleQueryHandler : IRequestHandler<GetMaintenancesByVehicleQuery, IReadOnlyList<MaintenanceDto>>
{
    private readonly IMaintenanceRepository _repository;
    private readonly IMapper _mapper;

    public GetMaintenancesByVehicleQueryHandler(IMaintenanceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<MaintenanceDto>> Handle(GetMaintenancesByVehicleQuery request, CancellationToken cancellationToken) 
    {
        var maintenanceEntityList = await _repository.GetByVehicleIdAsync(request.VehicleId, cancellationToken);

        return _mapper.Map<IReadOnlyList<MaintenanceDto>>(maintenanceEntityList);
    }
}
