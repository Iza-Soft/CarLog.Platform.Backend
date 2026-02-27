using System.Text.RegularExpressions;
using CarLog.Vehicle.Domain.Validators.Abstractions;

namespace CarLog.Vehicle.Domain.Validators.Europe;

public sealed class GreekPlateValidator : ICountryPlateValidator
{
    public string CountryCode => "GR";

    private static readonly Regex _grPlateRegex = new(
        @"^[A-Z]{3}\d{4}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase,
        TimeSpan.FromMilliseconds(100));

    public bool IsValid(string plateNumber)
        => _grPlateRegex.IsMatch(plateNumber);
}
