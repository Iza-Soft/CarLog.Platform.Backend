using System.ComponentModel.DataAnnotations;

namespace CarLog.Vehicle.Domain.Enums;

public enum OwnerType : byte
{
    Unknown = 0,

    [Display(Name = "Personal", Description = "Individual owner - B2C")]
    Personal = 1,

    [Display(Name = "Corporate", Description = "Company owned - B2B")]
    Corporate = 2
}
