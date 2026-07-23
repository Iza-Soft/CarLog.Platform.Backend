using System.ComponentModel.DataAnnotations;

namespace CarLog.Maintenance.Domain.Enums;

public enum MaintenanceType : byte
{
    [Display(Name = "Unknown", Description = "Maintenance type not specified")]
    Unknown = 0,

    [Display(Name = "Oil Change", Description = "Engine oil and filter replacement")]
    OilChange = 1,

    [Display(Name = "Tire Change", Description = "Tire replacement or rotation")]
    TireChange = 2,

    [Display(Name = "Inspection", Description = "Periodic technical inspection")]
    Inspection = 3,

    [Display(Name = "Other", Description = "Other maintenance type")]
    Other = 99,
}
