using AutoMapper;
using CarLog.Reminder.Application.DTOs;
using CarLog.Reminder.Domain.Entities;

namespace CarLog.Reminder.Application.Mappings;

public class ReminderMappingProfile : Profile
{
    public ReminderMappingProfile()
    {
        CreateMap<ReminderEntity, ReminderDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ForMember(dest => dest.IsOverdue, opt => opt.MapFrom(src => src.IsOverdue(DateOnly.FromDateTime(DateTime.UtcNow), null)));
    }
}