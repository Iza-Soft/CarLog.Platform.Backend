using System.Text.RegularExpressions;
using CarLog.Vehicle.Domain.Validators.Abstractions;

namespace CarLog.Vehicle.Domain.Validators.Europe;

public sealed class SpanishPlateValidator : ICountryPlateValidator
{
    public string CountryCode => "FR";

    private static readonly Regex _esPlateRegex = new(
        @"^\d{4}[A-Z]{3}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase,
        TimeSpan.FromMilliseconds(100));

    public bool IsValid(string plateNumber)
        => _esPlateRegex.IsMatch(plateNumber);
}
