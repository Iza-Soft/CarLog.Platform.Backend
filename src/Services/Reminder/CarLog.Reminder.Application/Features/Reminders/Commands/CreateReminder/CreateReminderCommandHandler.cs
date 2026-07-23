using AutoMapper;
using CarLog.Reminder.Application.Common.Interfaces;
using CarLog.Reminder.Application.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CarLog.Reminder.Application.Features.Reminders.Commands.CreateReminder;

public class CreateReminderCommandHandler : IRequestHandler<CreateReminderCommand, ReminderDto>
{
    private readonly IReminderRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateReminderCommandHandler> _logger;

    public CreateReminderCommandHandler(IReminderRepository repository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateReminderCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ReminderDto> Handle(CreateReminderCommand request, CancellationToken cancellationToken)
    {
        var reminder = Domain.Entities.ReminderEntity.Create(
            request.VehicleId,
            request.Type,
            request.Description,
            request.DueDate,
            request.DueMileage);

        await _repository.AddAsync(reminder, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created reminder {ReminderId} for vehicle {VehicleId}, type {Type}, due date {DueDate}, due mileage {DueMileage}", reminder.Id, request.VehicleId, request.Type, request.DueDate, request.DueMileage);

        return _mapper.Map<ReminderDto>(reminder);
    }
}