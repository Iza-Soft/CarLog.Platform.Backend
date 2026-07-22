using AutoMapper;
using CarLog.Maintenance.Application.Common.Exceptions;
using CarLog.Maintenance.Application.Common.Interfaces;
using CarLog.Maintenance.Application.DTOs;
using CarLog.Maintenance.Domain.Entities;
using MediatR;

namespace CarLog.Maintenance.Application.Features.Maintenances.Queries.GetMaintenanceById;

public class GetMaintenanceByIdQueryHandler : IRequestHandler<GetMaintenanceByIdQuery, MaintenanceDto>
{
    private readonly IMaintenanceRepository _repository;
    private readonly IMapper _mapper;

    public GetMaintenanceByIdQueryHandler(IMaintenanceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<MaintenanceDto> Handle(GetMaintenanceByIdQuery request, CancellationToken cancellationToken) 
    {
        var maintenanceEntity = await _repository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException(nameof(MaintenanceEntity), request.Id);

        return _mapper.Map<MaintenanceDto>(maintenanceEntity);
    }
}
