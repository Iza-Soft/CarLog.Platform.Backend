using AutoMapper;
using MediatR;
using CarLog.Reminder.Application.Common.Exceptions;
using CarLog.Reminder.Application.Common.Interfaces;
using CarLog.Reminder.Application.DTOs;

namespace CarLog.Reminder.Application.Features.Reminders.Queries.GetReminderById;

public class GetReminderByIdQueryHandler : IRequestHandler<GetReminderByIdQuery, ReminderDto>
{
    private readonly IReminderRepository _repository;
    private readonly IMapper _mapper;

    public GetReminderByIdQueryHandler(IReminderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ReminderDto> Handle(GetReminderByIdQuery request, CancellationToken cancellationToken)
    {
        var reminder = await _repository.GetByIdAsync(request.Id, cancellationToken) ?? throw new NotFoundException(nameof(Domain.Entities.ReminderEntity), request.Id);

        return _mapper.Map<ReminderDto>(reminder);
    }
}