using System.Text.RegularExpressions;
using CarLog.Vehicle.Domain.Validators.Abstractions;

namespace CarLog.Vehicle.Domain.Validators.Balkans;

public sealed class SerbianPlateValidator : ICountryPlateValidator
{
    public string CountryCode => "RS";

    private static readonly Regex _rsPlateRegex = new(
        @"^[A-Z]{2}\d{3,4}[A-Z]{2}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase,
        TimeSpan.FromMilliseconds(100));

    public bool IsValid(string plateNumber)
        => _rsPlateRegex.IsMatch(plateNumber);
}