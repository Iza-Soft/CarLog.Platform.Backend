using AutoMapper;
using CarLog.Maintenance.Application.Common.Exceptions;
using CarLog.Maintenance.Application.Common.Interfaces;
using CarLog.Maintenance.Application.DTOs;
using CarLog.Maintenance.Domain.Entities;
using CarLog.Maintenance.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CarLog.Maintenance.Application.Features.Maintenances.Commands.UpdateMaintenance;

public class UpdateMaintenanceCommandHandler : IRequestHandler<UpdateMaintenanceCommand, MaintenanceDto>
{
    private readonly IMaintenanceRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateMaintenanceCommandHandler> _logger;

    public UpdateMaintenanceCommandHandler(IMaintenanceRepository repository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateMaintenanceCommandHandler> logger) 
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<MaintenanceDto> Handle(UpdateMaintenanceCommand request, CancellationToken cancellationToken) 
    {
        var maintenanceEntity = await _repository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException(nameof(MaintenanceEntity), request.Id);

        var cost = Money.Create(request.CostAmount, request.CostCurrency);

        maintenanceEntity.Update(request.Type, request.Description, request.ServiceDate, request.MileageAtService, cost);

        _repository.Update(maintenanceEntity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Updated maintenance record {MaintenanceId} for car {VehicleId}, new type {Type}, new amount {CostAmount} {CostCurrency}", maintenanceEntity.Id, maintenanceEntity.VehicleId, request.Type, request.CostAmount, request.CostCurrency);

        return _mapper.Map<MaintenanceDto>(maintenanceEntity);
    }
}
