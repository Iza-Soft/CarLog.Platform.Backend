using System.Text.RegularExpressions;
using CarLog.Vehicle.Domain.Validators.Abstractions;

namespace CarLog.Vehicle.Domain.Validators.Europe;

public sealed class RomanianPlateValidator : ICountryPlateValidator
{
    public string CountryCode => "RO";

    private static readonly Regex _roPlateRegex = new(
        @"^[A-Z]{2}\d{2,3}[A-Z]{3}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase,
        TimeSpan.FromMilliseconds(100));

    public bool IsValid(string plateNumber)
        => _roPlateRegex.IsMatch(plateNumber);
}
