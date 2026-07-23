using AutoMapper;
using CarLog.Reminder.Application.Common.Exceptions;
using CarLog.Reminder.Application.Common.Interfaces;
using CarLog.Reminder.Application.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CarLog.Reminder.Application.Features.Reminders.Commands.UpdateReminder;

public class UpdateReminderCommandHandler : IRequestHandler<UpdateReminderCommand, ReminderDto>
{
    private readonly IReminderRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateReminderCommandHandler> _logger;

    public UpdateReminderCommandHandler(IReminderRepository repository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateReminderCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ReminderDto> Handle(UpdateReminderCommand request, CancellationToken cancellationToken)
    {
        var reminder = await _repository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException(nameof(Domain.Entities.ReminderEntity), request.Id);

        reminder.Update(request.Type, request.Description, request.DueDate, request.DueMileage);

        _repository.Update(reminder);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Updated reminder {ReminderId} for vehicle {VehicleId}", reminder.Id, reminder.VehicleId);

        return _mapper.Map<ReminderDto>(reminder);
    }
}