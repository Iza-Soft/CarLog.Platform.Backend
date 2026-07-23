using System.ComponentModel.DataAnnotations;

namespace CarLog.Reminder.Domain.Enums;

public enum ReminderType : byte
{
    [Display(Name = "Unknown", Description = "Reminder type not specified")]
    Unknown = 0,

    [Display(Name = "Oil Change", Description = "Upcoming oil change")]
    OilChange = 1,

    [Display(Name = "Tire Change", Description = "Upcoming tire change or rotation")]
    TireChange = 2,

    [Display(Name = "Brake Service", Description = "Upcoming brake inspection or service")]
    BrakeService = 3,

    [Display(Name = "Inspection", Description = "Periodic technical inspection (ГО)")]
    Inspection = 4,

    [Display(Name = "Insurance", Description = "Insurance policy renewal")]
    Insurance = 5,

    [Display(Name = "Registration", Description = "Vehicle registration or road tax renewal")]
    Registration = 6,

    [Display(Name = "Vignette", Description = "Road toll vignette renewal")]
    Vignette = 7,

    [Display(Name = "Other", Description = "Any other upcoming vehicle-related reminder")]
    Other = 99
}