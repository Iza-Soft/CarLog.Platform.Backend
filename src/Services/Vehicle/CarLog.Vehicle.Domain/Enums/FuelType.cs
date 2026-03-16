using System.ComponentModel.DataAnnotations;

namespace CarLog.Vehicle.Domain.Enums;

public enum FuelType : byte
{
    Unknown = 0,

    [Display(Name = "Petrol", Description = "Gasoline engine")]
    Petrol = 1,

    [Display(Name = "Diesel", Description = "Diesel engine")]
    Diesel = 2,

    [Display(Name = "Electric", Description = "Battery electric vehicle")]
    Electric = 3,

    [Display(Name = "Hybrid", Description = "Petrol + Electric")]
    Hybrid = 4,

    [Display(Name = "Plug-in Hybrid", Description = "Hybrid with external charging")]
    PlugInHybrid = 5,

    [Display(Name = "LPG", Description = "Liquified Petroleum Gas")]
    LPG = 6,

    [Display(Name = "Hydrogen", Description = "Fuel cell vehicle")]
    CNG = 7
}
