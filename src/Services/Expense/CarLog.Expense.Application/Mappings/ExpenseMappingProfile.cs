using AutoMapper;
using CarLog.Expense.Application.DTOs;
using CarLog.Expense.Domain.Entities;

namespace CarLog.Expense.Application.Mappings;

public class ExpenseMappingProfile : Profile
{
    public ExpenseMappingProfile()
    {
        CreateMap<ExpenseEntity, ExpenseDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ForMember(dest => dest.AmountValue, opt => opt.MapFrom(src => src.Amount.Amount))
            .ForMember(dest => dest.AmountCurrency, opt => opt.MapFrom(src => src.Amount.Currency));
    }
}