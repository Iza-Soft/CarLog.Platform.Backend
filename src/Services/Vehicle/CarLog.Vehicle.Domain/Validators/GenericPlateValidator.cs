using System.Text.RegularExpressions;
using CarLog.Vehicle.Domain.Validators.Abstractions;

namespace CarLog.Vehicle.Domain.Validators;

public sealed class GenericPlateValidator : ICountryPlateValidator
{
    public string CountryCode => "XX"; // Generic

    private static readonly Regex _genericRegex = new(
        @"^[A-Z0-9\s\-]{2,12}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase,
        TimeSpan.FromMilliseconds(100));

    public bool IsValid(string plateNumber)
        => _genericRegex.IsMatch(plateNumber);
}