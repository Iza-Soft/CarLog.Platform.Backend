using AutoMapper;
using CarLog.Maintenance.Application.Common.Interfaces;
using CarLog.Maintenance.Application.DTOs;
using CarLog.Maintenance.Domain.Entities;
using CarLog.Maintenance.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CarLog.Maintenance.Application.Features.Maintenances.Commands.CreateMaintenancе;

public class CreateMaintenanceCommandHandler : IRequestHandler<CreateMaintenanceCommand, MaintenanceDto>
{
    private readonly IMaintenanceRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    public readonly IMapper _mapper;
    private readonly ILogger<CreateMaintenanceCommandHandler> _logger;

    public CreateMaintenanceCommandHandler(IMaintenanceRepository repository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateMaintenanceCommandHandler> logger) 
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<MaintenanceDto> Handle(CreateMaintenanceCommand request, CancellationToken cancellationToken) 
    {
        var cost = Money.Create(request.CostAmount, request.CostCurrency);

        var maintenanceEntity = MaintenanceEntity.Create(
            request.VehicleId,
            request.Type,
            request.Description,
            request.ServiceDate,
            request.MileageAtService,
            cost);

        await _repository.AddAsync(maintenanceEntity, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created maintenance record {MaintenanceId} for car {VehicleId}, type {Type}, amount {CostAmount} {CostCurrency}", maintenanceEntity.Id, request.VehicleId, request.Type, request.CostAmount, request.CostCurrency);

        return _mapper.Map<MaintenanceDto>(maintenanceEntity);
    }
}
