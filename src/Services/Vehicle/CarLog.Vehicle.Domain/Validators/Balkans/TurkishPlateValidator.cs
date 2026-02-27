using System.Text.RegularExpressions;
using CarLog.Vehicle.Domain.Validators.Abstractions;

namespace CarLog.Vehicle.Domain.Validators.Balkans;

public sealed class TurkishPlateValidator : ICountryPlateValidator
{
    public string CountryCode => "TR";

    private static readonly Regex _trPlateRegex = new(
        @"^\d{2}[A-Z]{1,3}\d{2,4}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase,
        TimeSpan.FromMilliseconds(100));

    public bool IsValid(string plateNumber)
        => _trPlateRegex.IsMatch(plateNumber);
}
