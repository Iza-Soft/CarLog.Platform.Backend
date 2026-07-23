using System.ComponentModel.DataAnnotations;

namespace CarLog.Expense.Domain.Enums;

public enum ExpenseType : byte
{
    [Display(Name = "Unknown", Description = "Expense type not specified")]
    Unknown = 0,

    [Display(Name = "Fuel", Description = "Petrol, diesel, gas, or electric charging costs")]
    Fuel = 1,

    [Display(Name = "Insurance", Description = "Vehicle insurance premiums")]
    Insurance = 2,

    [Display(Name = "Parking", Description = "Parking fees")]
    Parking = 3,

    [Display(Name = "Toll", Description = "Road tolls and vignettes")]
    Toll = 4,

    [Display(Name = "Fine", Description = "Traffic fines and penalties")]
    Fine = 5,

    [Display(Name = "Registration", Description = "Vehicle registration and road tax")]
    Registration = 6,

    [Display(Name = "Other", Description = "Any other vehicle-related expense")]
    Other = 99
}