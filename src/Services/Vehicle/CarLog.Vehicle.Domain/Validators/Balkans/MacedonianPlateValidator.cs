using System.Text.RegularExpressions;
using CarLog.Vehicle.Domain.Validators.Abstractions;

namespace CarLog.Vehicle.Domain.Validators.Balkans;

public sealed class MacedonianPlateValidator : ICountryPlateValidator
{
    public string CountryCode => "MK";

    private static readonly Regex _mkPlateRegex = new(
        @"^[A-Z]{2}\d{4}[A-Z]{2}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase,
        TimeSpan.FromMilliseconds(100));

    public bool IsValid(string plateNumber)
        => _mkPlateRegex.IsMatch(plateNumber);
}
