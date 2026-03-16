using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CarLog.Vehicle.Domain.Enums;

public enum VehicleType : byte
{
    Unknown = 0,

    [Display(Name = "Sedan", Description = "4-door passenger car")]
    Sedan = 1,

    [Display(Name = "SUV", Description = "Sport Utility Vehicle")]
    SUV = 2,

    [Display(Name = "Hatchback", Description = "Car with rear door that swings upward")]
    Hatchback = 3,

    [Display(Name = "Coupe", Description = "2-door sports car")]
    Coupe = 4,

    [Display(Name = "Convertible", Description = "Car with retractable roof")]
    Convertible = 5,

    [Display(Name = "Wagon", Description = "Extended rear cargo area")]
    Wagon = 6,

    [Display(Name = "Van", Description = "Passenger or cargo van")]
    Van = 7,

    [Display(Name = "Truck", Description = "Pickup truck or lorry")]
    Truck = 8,

    [Display(Name = "Motorcycle", Description = "Two-wheeled motor vehicle")]
    Motorcycle = 9,

    [Display(Name = "Bus", Description = "Passenger bus")]
    Bus = 10,

    [Display(Name = "Other", Description = "Other vehicle type")]
    Other = 99
}
